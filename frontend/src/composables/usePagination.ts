import { computed, ref } from 'vue'

export function usePagination(defaultPageSize = 20) {
  const page = ref(1)
  const pageSize = ref(defaultPageSize)
  const total = ref(0)

  const paginationParams = computed(() => ({
    page: page.value,
    page_size: pageSize.value,
  }))

  const resetPage = () => {
    page.value = 1
  }

  const handlePageChange = (newPage: number) => {
    page.value = newPage
  }

  const handleSizeChange = (newSize: number) => {
    pageSize.value = newSize
    page.value = 1
  }

  return {
    page,
    pageSize,
    total,
    paginationParams,
    resetPage,
    handlePageChange,
    handleSizeChange,
  }
}
