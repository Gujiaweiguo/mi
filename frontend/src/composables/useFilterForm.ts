import { computed, reactive } from 'vue'

export const useFilterForm = <TFilters extends Record<string, string>>(initialFilters: TFilters) => {
  const filters = reactive({ ...initialFilters }) as TFilters
  const filterKeys = Object.keys(initialFilters) as Array<keyof TFilters>

  const isDirty = computed(() =>
    filterKeys.some((key) => filters[key] !== initialFilters[key]),
  )

  const reset = () => {
    for (const key of filterKeys) {
      ;(filters as Record<keyof TFilters, string>)[key] = initialFilters[key]
    }
  }

  return {
    filters,
    isDirty,
    reset,
  }
}
