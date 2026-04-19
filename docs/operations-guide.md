# MI 运维手册

面向系统管理员和运维人员的日常操作参考。本手册覆盖部署、日常操作、监控、备份恢复和故障排查。

关联文档：[deployment-defaults.md](deployment-defaults.md)、[deployment-topology.md](deployment-topology.md)、[cutover-runbook.md](cutover-runbook.md)、[go-live-checklist.md](go-live-checklist.md)

---

## 1. 系统概述

### 架构

MI 是一套物业管理信息系统，采用前后端分离架构：

- **Frontend**: Vue 3 SPA (Vite + Element Plus)
- **Backend**: Go modular monolith (Gin + Gorm)
- **Database**: MySQL 8
- **Reverse Proxy**: Nginx
- **Deployment**: Docker Compose

### 服务清单

| 服务 | 说明 | 备注 |
|------|------|------|
| mysql | MySQL 8 数据库 | healthcheck: `mysqladmin ping` |
| backend | Go API 服务 | healthcheck: `/healthz` |
| frontend | 静态资源服务 (nginx) | healthcheck: `/` |
| nginx | 反向代理入口 | `/api/*` → backend, `/*` → frontend |
| migrate | 数据库迁移 (一次性) | 运行后自动退出 |
| backup | 定时备份 sidecar | 需启用 `--profile backup` |

### 端口映射

| 用途 | 开发环境默认 | 生产环境默认 |
|------|-------------|-------------|
| HTTP 入口 | `:5180` (直连 backend) | `:80` (nginx) |
| Frontend dev | `:5173` | 容器内部 |
| Backend API | `:5180` | `:5180` (内部) |
| MySQL | `:3306` | 不对外暴露 |
| Prometheus | `:9090` | 仅开发环境 |
| Grafana | `:3000` | 仅开发环境 |

---

## 2. 环境搭建

所有 Compose 命令的工作目录为 `deploy/compose`。

### 开发环境（仅 MySQL）

最常用模式：本地跑 backend 和 frontend，数据库用 Docker。

```bash
docker compose --env-file ../env/dev.env \
  -f docker-compose.dev.yml up mysql
```

然后分别在本地启动 backend 和 frontend。

### 全栈开发环境

所有服务含热重载：

```bash
docker compose --env-file ../env/dev.env \
  -f docker-compose.dev.yml --profile fullstack up
```

运行数据库迁移：

```bash
docker compose --env-file ../env/dev.env \
  -f docker-compose.dev.yml run --rm migrate
```

### 监控栈

在 fullstack 基础上启用 Prometheus + Grafana：

```bash
docker compose --env-file ../env/dev.env \
  -f docker-compose.dev.yml --profile fullstack --profile monitoring up
```

- Prometheus: http://localhost:9090
- Grafana: http://localhost:3000 (admin/admin)

### 生产环境

首次部署前，先确认 `deploy/env/production.env` 中的占位符密码已替换为真实值：

- `MYSQL_PASSWORD`
- `MYSQL_ROOT_PASSWORD`
- `MI_AUTH_JWT_SECRET`

占位符值 (`change-me`) 会被 preflight 脚本拦截。

启动生产栈：

```bash
docker compose --env-file ../env/production.env \
  -f docker-compose.production.yml up -d
```

启用定时备份（每日 02:00 CST 自动执行）：

```bash
docker compose --env-file ../env/production.env \
  -f docker-compose.production.yml --profile backup up -d
```

生产环境启动时，`migrate` 服务自动运行迁移，`backend` 等待迁移完成后才启动。

---

## 3. 日常操作

### 启动/停止服务

```bash
# 启动（后台运行）
docker compose --env-file ../env/<env>.env -f docker-compose.<env>.yml up -d

# 停止
docker compose --env-file ../env/<env>.env -f docker-compose.<env>.yml down

# 停止并清除数据卷（慎用）
docker compose --env-file ../env/<env>.env -f docker-compose.<env>.yml down -v
```

### 查看日志

```bash
docker compose logs -f backend          # 实时跟踪
docker compose logs --tail 100 backend  # 最近 100 行
```

生产环境日志使用 json-file driver，单文件上限 10MB，保留 3 个轮转。

### 数据库迁移

生产环境迁移在启动时自动执行。如需手动运行：

```bash
docker compose --env-file ../env/production.env \
  -f docker-compose.production.yml run --rm migrate
```

### 健康检查

```bash
curl http://localhost:5180/healthz     # 直连 backend
curl http://localhost/api/healthz      # 通过 nginx
```

healthcheck 间隔 10 秒，超时 5 秒。

### Preflight 检查

```bash
scripts/compose-preflight.sh production
```

验证配置文件、运行时目录、占位符密码替换状态。

---

## 4. 监控

> 监控栈仅在开发环境通过 `--profile monitoring` 启用。生产环境如需监控，需另行部署。

### 数据采集

Prometheus 每 15 秒从 `backend:5180/metrics` 抓取指标。原始指标可通过 `curl http://localhost:5180/metrics` 查看。

### 关键指标

| 指标 | 说明 |
|------|------|
| `http_requests_total` | HTTP 请求计数（按 status/method/path 分组） |
| `http_request_duration_seconds` | 请求耗时直方图 |
| `http_requests_in_flight` | 当前进行中的请求数 |

### Grafana 仪表盘

http://localhost:3000，默认 `admin/admin`。Dashboard 配置位于 `deploy/monitoring/grafana/dashboards/`。

### 告警规则

| 告警名称 | 条件 | 级别 | 阈值 |
|----------|------|------|------|
| BackendHighErrorRate | 5xx 比率超限 | critical | > 5% 持续 5 分钟 |
| BackendHighLatency | p95 延迟超限 | warning | > 2s 持续 5 分钟 |
| BackendNoMetrics | 抓取失败 | critical | 持续 3 分钟 |

告警规则定义在 `deploy/monitoring/alerts.yml`。

---

## 5. 备份与恢复

### 手动备份

```bash
scripts/db-backup.sh production
```

备份内容包括：

- MySQL 逻辑 dump
- 运行时 logs、documents、uploads 快照
- 后端配置文件
- Compose 环境变量文件

备份文件写入：

```
artifacts/backups/production/mi-production-backup-<timestamp>.tar.gz
```

### 自动备份

启用 backup sidecar 后，每日 02:00 CST 自动执行。挂载路径通过 `MI_RUNTIME_BACKUPS` 配置，默认 `./artifacts/backups/production`。

### 恢复

仅恢复数据库：

```bash
scripts/db-restore.sh production <backup-archive>
```

恢复数据库 + 运行时文件（logs/documents/uploads/config）：

```bash
scripts/db-restore.sh production <backup-archive> --restore-runtime-files
```

> 恢复操作会覆盖当前数据，执行前确认已做好二次备份。

### 生产备份路径

默认挂载位置：

```
deploy/runtime/production/    ← 运行时数据根目录
  ├── mysql/                  ← MySQL 数据文件
  ├── logs/                   ← 后端日志
  ├── documents/              ← 生成的文档
  └── uploads/                ← 上传文件
```

---

## 6. 故障排查

### 服务启动失败

1. 查看日志：`docker compose logs <service>`
2. 检查端口是否被占用：`ss -tlnp | grep <port>`
3. 检查运行时目录是否可写：`ls -la deploy/runtime/production/`

### 数据库连接失败

1. 确认 mysql 容器健康：`docker compose ps mysql`
2. 检查环境变量中的数据库凭据
3. 查看 mysql 错误日志：`docker compose logs mysql`

### 前端无法访问后端

1. 确认 backend 健康检查通过
2. 确认 nginx 配置 `/api/` → `backend:5180`：`curl http://localhost/api/healthz`
3. 如果 nginx 正常但 backend 不可达，检查容器网络

### API 返回 429

触发了 rate limit，默认 **100 req/s per IP**。需提高吞吐时修改 backend 配置。

### 监控指标异常

检查 Prometheus targets 页面 (http://localhost:9090/targets)，确认 `mi-backend` 状态为 UP。`BackendNoMetrics` 触发时优先确认 backend 存活。

### 文档生成或上传失败

检查运行时目录权限。容器需要对 `deploy/runtime/production/documents/` 和 `deploy/runtime/production/uploads/` 有写权限。

### Migrate 服务失败

migrate 失败会阻止 backend 启动。查看日志 `docker compose logs migrate`，常见原因为 MySQL 未就绪或凭据错误。修复后重新运行 `docker compose run --rm migrate`。

---

## 7. API 文档

Swagger UI 地址：

```
http://localhost:5180/docs/index.html
```

覆盖 **97 个 API 端点**，**14 个业务分组**：auth、org、property、lease、bill/invoice、workflow、tax、report、document、excel 等。

> 生产环境通过 nginx 访问时，Swagger UI 地址为 `http://<host>/api/docs/index.html`。
