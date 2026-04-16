package reporting

import "fmt"

func reportR01Columns() []Column {
	return columns(
		"store_name", "门店",
		"department_name", "部门",
		"rent_status", "出租状态",
		"use_area_total", "使用面积汇总",
	)
}

func reportR02Columns() []Column {
	return columns(
		"lease_no", "合同编号",
		"customer_code", "客户编号",
		"customer_name", "客户名称",
		"trade_name", "业态",
		"management_type_name", "经营方式",
		"unit_code", "商铺编码",
		"unit_name", "商铺名称",
		"rent_area", "租赁面积",
		"brand_name", "品牌",
		"shop_type_name", "店铺类型",
		"department_name", "分公司",
		"store_name", "门店",
	)
}

func reportR03Columns() []Column {
	return columns(
		"shop_type_name", "店铺类型",
		"rent_area", "租赁面积",
		"current_sales", "本期销售额",
		"same_period_sales", "同期销售额",
		"comparable_sales", "可比销售额",
		"monthly_rent", "租金",
	)
}

func reportR04Columns() []Column {
	cols := columns(
		"unit_code", "商铺编码",
		"unit_name", "商铺名称",
		"rent_area", "租赁面积",
		"shop_type", "业态",
		"total_sales", "总销售额",
	)
	for day := 1; day <= 31; day++ {
		cols = append(cols, Column{Key: fmt.Sprintf("day_%02d", day), Label: fmt.Sprintf("%d日", day)})
	}
	return cols
}

func reportR05Columns() []Column {
	return columns(
		"unit_code", "铺位编码",
		"floor_area", "建筑面积",
		"budget_unit_price", "预算单价",
		"current_lease_price", "当前租赁单价",
		"potential_customer", "意向客户",
		"prospect_brand", "招商品牌",
		"prospect_trade", "招商业态",
		"average_ticket", "平均客单",
		"prospect_rent_price", "招商租价",
		"rent_increment", "递增条件",
		"prospect_term_months", "租期",
	)
}

func reportR06Columns() []Column {
	return columns(
		"store_name", "门店",
		"period", "期间",
		"period_receivable", "本期应收租金",
		"period_received", "本期实收租金",
		"monthly_budget", "月预算",
		"annual_budget", "年预算",
		"ytd_cumulative", "累计完成额",
	)
}

func reportR07Columns() []Column {
	cols := columns(
		"store_name", "门店",
		"brand_name", "品牌",
		"annual_total", "年度合计",
	)
	for month := 1; month <= 12; month++ {
		cols = append(cols, Column{Key: fmt.Sprintf("month_%02d", month), Label: fmt.Sprintf("%d月", month)})
	}
	return cols
}

func reportR10Columns() []Column {
	cols := columns(
		"store_name", "门店",
		"year", "年度",
		"annual_total", "月度总客流",
	)
	for month := 1; month <= 12; month++ {
		cols = append(cols, Column{Key: fmt.Sprintf("month_%02d", month), Label: fmt.Sprintf("%d月", month)})
	}
	return cols
}

func reportR11Columns() []Column {
	return columns(
		"store_name", "门店",
		"period", "期间",
		"leased_area", "已出租面积",
		"total_area", "总面积",
	)
}

func reportR12Columns() []Column {
	return columns(
		"store_name", "门店",
		"period", "期间",
		"shop_type_name", "店铺类型",
		"occupancy_status", "单元状态",
		"area_total", "面积汇总",
	)
}

func reportR13Columns() []Column {
	return columns(
		"store_name", "门店",
		"shop_type_name", "店铺类型",
		"period", "月份",
		"current_sales", "本期销售额",
		"ytd_sales", "年累计销售额",
		"prev_month_sales", "上月销售额",
		"last_year_ytd_sales", "去年累计销售额",
	)
}

func reportR14Columns() []Column {
	return columns(
		"store_name", "门店",
		"shop_type_name", "店铺类型",
		"period", "月份",
		"sales_amount", "销售额",
		"area_total", "面积",
		"days_in_month", "当月天数",
		"efficiency", "坪效",
	)
}

func reportR15Columns() []Column {
	return columns(
		"store_name", "门店",
		"shop_type_name", "店铺类型",
		"period", "月份",
		"sales_amount", "销售额",
		"rent_income", "租金收入",
	)
}

func reportR18Columns() []Column {
	return columns(
		"customer_name", "客户",
		"store_name", "门店",
		"unit_name", "商铺",
		"brand_name", "品牌",
		"period", "月份",
		"rent_area", "租赁面积",
		"current_sales", "本期销售",
		"comparable_sales", "可比销售",
		"same_period_sales", "同期销售",
		"period_receivable", "本期应收",
		"period_received", "本期实收",
		"period_arrears", "本期欠费",
		"cumulative_receivable", "累计应收",
		"cumulative_arrears", "累计欠费",
		"days_in_month", "当月天数",
		"efficiency", "坪效",
	)
}

func reportR19Columns() []Column {
	return columns(
		"unit_code", "商铺编码",
		"unit_name", "商铺描述",
		"floor_area", "建筑面积",
		"rent_area", "租赁面积",
		"rent_status", "出租状态",
		"brand_name", "品牌",
		"customer_name", "客户",
		"shop_type_name", "店铺类型",
		"pos_x", "X坐标",
		"pos_y", "Y坐标",
		"color_hex", "颜色",
	)
}

func agingCustomerColumns(includeChargeType bool) []Column {
	columns := []Column{{Key: "unit_collection", Label: "商铺集合"}, {Key: "customer_name", Label: "客户"}, {Key: "trade_name", Label: "业态"}, {Key: "department_name", Label: "分公司"}, {Key: "lease_no", Label: "合同"}, {Key: "deposit_amount", Label: "押金"}}
	if includeChargeType {
		columns = append(columns, Column{Key: "charge_type", Label: "费用项目"})
	}
	return append(columns, agingBucketColumns()...)
}

func agingDepartmentColumns(includeChargeType bool) []Column {
	columns := []Column{{Key: "department_name", Label: "分公司"}, {Key: "deposit_amount", Label: "押金"}}
	if includeChargeType {
		columns = append(columns, Column{Key: "charge_type", Label: "费用项目"})
	}
	return append(columns, agingBucketColumns()...)
}

func agingBucketColumns() []Column {
	return columns(
		"within_one_month", "1月内",
		"one_to_two_months", "1-2月",
		"two_to_three_months", "2-3月",
		"three_to_six_months", "3-6月",
		"six_to_nine_months", "6-9月",
		"nine_to_twelve_months", "9-12月",
		"one_to_two_years", "1-2年",
		"two_to_three_years", "2-3年",
		"over_three_years", "3年以上",
		"total", "合计",
	)
}

func columns(pairs ...string) []Column {
	result := make([]Column, 0, len(pairs)/2)
	for i := 0; i < len(pairs); i += 2 {
		result = append(result, Column{Key: pairs[i], Label: pairs[i+1]})
	}
	return result
}
