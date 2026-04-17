## 1. 修正 unit evidence producer 行为

- [x] 1.1 修改 `scripts/verification/run-unit.sh`，将 backend 和 frontend 测试命令分别用 `set +e` 包裹、捕获各自退出码，确保即使一个 suite 失败另一个仍会执行。
- [x] 1.2 将 `--status passed` 替换为基于实际退出码计算的 `--status "$UNIT_STATUS"`，并在写完 evidence 后返回原始非零退出码。

## 2. 修正 integration evidence producer 行为

- [x] 2.1 修改 `scripts/verification/run-integration.sh`，用 `set +e` 包裹测试命令并捕获退出码。
- [x] 2.2 将 `--status passed` 替换为基于实际退出码计算的 `--status "$INTEGRATION_STATUS"`，并在写完 evidence 后返回原始非零退出码。

## 3. 验证与自测覆盖

- [x] 3.1 更新或新增验证自测用例，证明 `run-unit.sh` 和 `run-integration.sh` 在测试失败时能正确产出 `status: "failed"` 的 evidence 文件，而不仅仅是缺失 evidence。
- [x] 3.2 为当前提交运行 required verification commands，记录 commit-scoped evidence 到 `artifacts/verification/<commit-sha>/unit.json` 和 `artifacts/verification/<commit-sha>/integration.json`，确保 CI gate 可用。
- [x] 3.3 为当前提交运行 archive-only verification coverage，记录 commit-scoped evidence 到 `artifacts/verification/<commit-sha>/e2e.json`，确保 release 和 archive gates 都使用当前 HEAD evidence。
