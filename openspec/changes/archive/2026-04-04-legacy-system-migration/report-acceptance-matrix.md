# First-Release Generalize Report Acceptance Matrix

This matrix defines the minimum acceptance contract for `R01-R19`.

## Usage Rules

- This is a **normalized acceptance directory** derived from extractable SQL/content in `阳光商业MI.net系统设计.doc`.
- If later implementation discovers the exact legacy display title differs, the implementation MAY rename the UI label, but it MUST preserve the mapped `Report ID`, business meaning, core fields, filters, and output behavior unless OpenSpec is updated.
- Unless a report is explicitly visual/print-oriented, the default first-release output form is:
  - on-screen query result table
  - Excel export (`.xlsx`)
- CSV export MAY be added, but `.xlsx` is the minimum accepted export format for tabular reports.

## Matrix

### R01 — 出租状态面积汇总表
- **Core fields**: 门店/部门, 出租状态, 使用面积汇总
- **Filters**: 期间, 门店/部门
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 能按期间统计不同出租状态的面积汇总
  - 总面积与明细口径一致

### R02 — 合同台账报表
- **Core fields**: 合同编号, 客户编号, 客户名称, 业态, 经营方式, 商铺编码, 商铺名称, 租赁面积, 品牌, 店铺类型, 分公司, 门店
- **Filters**: 合同状态, 门店, 分公司, 经营方式, 客户, 业态
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 合同状态为有效口径时可正确返回台账记录
  - 合同、客户、商铺、品牌、门店信息关联正确

### R03 — 业态销售租金分析表
- **Core fields**: 店铺类型, 租赁面积, 本期销售额, 可比销售额, 同期销售额, 租金
- **Filters**: 统计月份, 门店, 店铺类型
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 能按店铺类型汇总销售和租金
  - 汇总口径与月销售数据一致

### R04 — 商铺日销售分析表
- **Core fields**: 商铺编码, 商铺名称, 租赁面积, 业态, 总销售额, 日销售列(1-31)
- **Filters**: 年, 月, 门店, 商铺
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 支持按自然月展开 1-31 日列
  - 月合计与各日汇总一致

### R05 — 铺位预算租价招商对比表
- **Core fields**: 铺位编码, 建筑面积, 预算单价, 当前租赁单价, 意向客户, 招商品牌/业态, 平均客单, 招商租价, 递增条件, 租期
- **Filters**: 年度, 门店, 楼层/区域, 铺位
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 能同时展示预算、现租、招商三类价格信息
  - 缺失现租或招商信息时可空值展示，不影响查询

### R06 — 门店租金预算执行表
- **Core fields**: 门店, 期间, 本期实收/应收租金, 月预算, 年预算, 累计完成额
- **Filters**: 年, 月/期间, 门店
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 能对比本期租金与预算值
  - 年累计口径与预算年累计口径一致

### R07 — 品牌年度销售分布表
- **Core fields**: 门店, 品牌, 年度合计, 1-12 月销售额
- **Filters**: 年度, 门店, 品牌
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 同一品牌年度合计等于 1-12 月汇总
  - 支持门店维度查看品牌分布

### R08 — 客户应收账龄汇总表
- **Core fields**: 商铺集合, 客户, 业态, 分公司, 合同, 押金, 1月内, 1-2月, 2-3月, 3-6月, 6-9月, 9-12月, 1-2年, 2-3年, 3年以上, 合计
- **Filters**: 截止期间, 分公司, 客户, 业态
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 账龄桶金额汇总等于合计
  - 押金与应收余额口径分离展示

### R09 — 客户应收账龄分费用表
- **Core fields**: 商铺集合, 客户, 业态, 分公司, 合同, 押金, 费用项目, 各账龄桶, 合计
- **Filters**: 截止期间, 分公司, 客户, 业态, 费用项目
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 费用项目维度可拆分账龄
  - 同客户同合同各费用项目合计可回溯到总账龄

### R10 — 客流年/月汇总表
- **Core fields**: 门店, 年度, 月度总客流, 1-12 月客流
- **Filters**: 年度, 门店
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 年度总客流等于 1-12 月汇总
  - 客流来源口径统一为 `trafficdata.InNum`

### R11 — 出租面积与总面积对比表
- **Core fields**: 门店, 期间, 已出租面积, 总面积
- **Filters**: 期间, 门店
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 已出租面积不大于总面积
  - 期间维度下口径一致

### R12 — 出租空置结构分析表
- **Core fields**: 门店, 城市/上级组织(如适用), 店铺类型, 期间, 单元状态, 面积汇总
- **Filters**: 期间, 门店, 店铺类型
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 至少区分出租/空置两类状态
  - 可按店铺类型统计面积结构

### R13 — 业态销售同比环比分析表
- **Core fields**: 门店, 店铺类型, 月份, 本期销售额, 年累计销售额, 上月销售额, 去年累计销售额
- **Filters**: 月份, 门店, 店铺类型
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 本期、上月、去年口径区分清楚
  - 支持按业态查看同比环比对比

### R14 — 坪效分析表
- **Core fields**: 门店, 店铺类型, 月份, 销售额, 面积, 当月天数, 坪效
- **Filters**: 月份, 门店, 店铺类型
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 坪效计算口径 = 销售额 / 天数 / 面积
  - 面积为空或零时需给出明确异常处理

### R15 — 销售与租金收入对比表
- **Core fields**: 门店, 店铺类型, 月份, 销售额, 租金收入
- **Filters**: 月份, 门店, 店铺类型
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 销售额和租金收入按相同期间对齐
  - 同门店同业态口径可交叉核对

### R16 — 分公司应收账龄汇总表
- **Core fields**: 分公司, 押金, 各账龄桶, 合计
- **Filters**: 截止期间, 分公司
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 分公司维度账龄合计正确
  - 可与客户账龄汇总结果交叉汇总

### R17 — 分公司应收账龄分费用表
- **Core fields**: 分公司, 费用项目, 押金, 各账龄桶, 合计
- **Filters**: 截止期间, 分公司, 费用项目
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 分公司 + 费用项目账龄拆分正确
  - 与分公司账龄汇总可做汇总核对

### R18 — 客户门店品牌经营综合分析表
- **Core fields**: 客户, 门店, 商铺, 品牌, 月份, 租赁面积, 本期销售, 可比销售, 同期销售, 本期应收/实收, 本期欠费, 累计应收, 累计欠费, 坪效类派生指标
- **Filters**: 月份, 门店, 客户, 品牌, 商铺
- **Output form**: 查询表格 + XLSX 导出
- **Acceptance checks**:
  - 同时展示经营表现和应收情况
  - 派生坪效指标按统一公式计算

### R19 — 商铺可视化分析输出
- **Core fields**: 商铺/铺位标识, 商铺编码, 商铺描述, 面积, 出租状态, 品牌, 客户, 方案/图层元数据
- **Filters**: 门店, 楼层, 区域, 图层/方案(如适用)
- **Output form**: 图形化输出/可视化页面；如文档要求，可附带结构化导出
- **Acceptance checks**:
  - 能按文档中的 `ShopXml` / `VAGraphic` 语义呈现铺位分析数据
  - 视觉对象与商铺/铺位基础数据能正确映射

## Cross-Report Acceptance Rules

- 同一统计口径下，汇总报表与明细/分项报表之间必须可交叉校验。
- 首版所有 Generalize 报表至少支持页面查询与 `.xlsx` 导出；`R19` 以可视化呈现为主。
- 所有金额/面积/销量类字段必须使用统一精度和空值处理规则。
- 所有报表必须支持权限控制，并记录查询/导出审计日志（如系统统一审计能力已落地）。
