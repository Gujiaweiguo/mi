## Context

`LeaseDetailView` 已经能展示合同主数据、生命周期操作、子类型明细，以及按 `lease_contract_id` 查询的 overtime bills，但它仍然缺少 operators 最关心的下游财务进展视图：该合同已经生成了哪些 charges、哪些 invoices、哪些 receivables。当前代码库里，billing charges、invoices、overtime bills 都已经支持按 `lease_contract_id` 查询；只有 receivables 还缺少同样的过滤能力，尽管底层数据模型和查询结果中已经包含 `lease_contract_id`。

这个变更需要同时覆盖：
- 前端 `LeaseDetailView` 的新下游聚合面板
- 前端 API 类型与调用扩展
- 后端 receivable list filter 的小范围能力补口

约束包括：
- 维持 first-release 范围，不引入新的业务子系统
- 尽量复用现有列表 API，而不是新增专用 lease summary endpoint
- 保持 Lease → Bill / Invoice 主链条在现有前后端架构内可验证

## Goals / Non-Goals

**Goals:**
- 让操作员在 `LeaseDetailView` 里直接看到与当前合同关联的 overtime bills、charges、invoices、receivables
- 为可跳转的下游记录提供快捷入口，减少跨页面人工搜索
- 用现有 `lease_contract_id` 查询能力支撑 charges / invoices / overtime，最小化后端新增面
- 为 receivables 增加 `lease_contract_id` 过滤，使 lease-context 查询行为与其他下游域保持一致
- 保持实现可测试：前端单测覆盖聚合面板加载与跳转，后端覆盖 receivable filter 解析与查询行为

**Non-Goals:**
- 不新增独立的 downstream timeline backend endpoint
- 不重做现有 overtime table 的业务操作流，只在其基础上补充全链路可见性
- 不实现跨模块批量操作或新的财务 mutation
- 不扩展到 first-release 之外的 email、自动催办、额外报表等能力

## Decisions

### 1. 在 `LeaseDetailView` 中新增“下游业务概览/时间线”面板，而不是新建独立页面

**Decision:** 将该能力放在现有合同详情页中，作为合同视角下的聚合面板。

**Why:** 操作员从合同进入后，最自然的问题就是“这个合同后续财务走到哪一步了”。把该视图放在 `LeaseDetailView` 内可以直接复用已加载的 lease context 和权限判断，也避免额外的导航成本。

**Alternatives considered:**
- **新建独立 downstream 页面**：会增加路由和上下文切换，不利于合同详情的一站式审查。
- **只做跳转按钮，不做本页聚合**：仍然要求操作员去各模块二次搜索，价值不足。

### 2. 复用现有列表 API，而不是新增专用 summary endpoint

**Decision:** charges、invoices、overtime 使用现有按 `lease_contract_id` 过滤的列表接口；receivables 补齐同样的 filter。

**Why:** 现有 API 已经覆盖大部分数据查询，新增专用 endpoint 会增加服务层、handler、DTO、测试和维护负担，而当前功能只需要摘要/列表信息，不需要新的聚合计算。

**Alternatives considered:**
- **新增 `/leases/:id/downstream` 聚合接口**：可以减少前端请求数，但对当前范围来说后端改动过大。
- **前端对 receivables 继续 client-side 过滤**：已有页面这样做过，但在 lease detail 这里会引入分页不一致与数据遗漏风险，不如补齐服务端过滤。

### 3. Receivables 过滤补口采用最小后端改动

**Decision:** 在 receivable query path 上增加 `lease_contract_id`：
- `frontend/src/api/invoice.ts` 的 `ListReceivablesParams`
- `backend/internal/http/handlers/invoice.go` query parse
- `backend/internal/invoice/model.go` 的 `ReceivableFilter`
- `backend/internal/invoice/ar_repository.go` 的 SQL WHERE 条件

**Why:** 底层表和响应对象已经有 `lease_contract_id` 字段，说明这是一个被明确建模但尚未暴露为过滤能力的缺口。补齐过滤即可让 lease-context downstream review 行为完整闭环。

**Alternatives considered:**
- **保持只按 customer/department 查询**：会让 lease detail 面板拿不到稳定、完整、可分页的合同级 receivable 列表。

### 4. UI 表现以“摘要卡 + 紧凑列表/时间序”优先，而不是完全展开型表格集合

**Decision:** 在 LeaseDetailView 中优先展示下游状态摘要和最近/关键记录列表，并为需要深挖的记录提供跳转。

**Why:** 合同详情页已经较长，若直接堆叠多个全量大表格，会显著增加滚动和阅读负担。该功能目标是“看清进展 + 快速跳转”，不是复制所有下游列表页。

**Alternatives considered:**
- **四个完整表格并排/串排**：信息完整但页面过重，超出当前详情页的信息密度阈值。

## Risks / Trade-offs

- **[Risk] Lease detail 页面变长、信息密度上升** → **Mitigation:** 使用摘要优先、列表次之、跳转补充的布局，而不是复制下游管理页全部列
- **[Risk] Receivables filter 改动触及后端查询行为** → **Mitigation:** 只追加可选过滤条件，不改变现有默认行为；补充单元/集成覆盖
- **[Risk] 多接口并行加载增加详情页等待时间** → **Mitigation:** 下游面板独立 loading/error 状态，不阻塞主合同详情渲染
- **[Risk] 跨模块跳转目标和筛选语义不一致** → **Mitigation:** 统一使用 `lease_contract_id` 作为主键语义，并尽量跳到已存在的 detail route

## Migration Plan

1. 先补 receivables 的 `lease_contract_id` 过滤能力与测试
2. 扩展前端 API 类型与调用参数
3. 在 `LeaseDetailView` 中增加下游摘要/时间线与快捷入口
4. 运行前端 lint/build/unit tests 与相关 e2e spot checks；运行后端相关测试
5. 如需回滚，可单独移除 lease detail 面板；receivable filter 为可选参数追加，不影响既有调用

## Open Questions

- LeaseDetailView 中 receivables 面板应显示“全部结果”还是只显示最近若干条并提供跳转入口？
- 对于 charges / invoices / receivables，是否需要直接跳转到列表页并带筛选参数，还是只保留 detail-page 跳转即可？
