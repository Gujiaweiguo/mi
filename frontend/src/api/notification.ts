import { http } from './http'
import type { PaginatedResponse } from './types'

export interface NotificationOutboxEntry {
  id: number
  event_type: string
  aggregate_type: string
  aggregate_id: number
  recipient_to: string
  recipient_cc: string
  subject: string
  template_name: string
  status: string
  attempt_count: number
  max_attempts: number
  sent_at: string | null
  last_error: string | null
  created_at: string
  updated_at: string
}

export interface ListNotificationsParams {
  page?: number
  page_size?: number
  event_type?: string
  aggregate_type?: string
  aggregate_id?: number
  status?: string
}

export const listNotifications = (params?: ListNotificationsParams) =>
  http.get<PaginatedResponse<NotificationOutboxEntry>>('/notifications', { params })
