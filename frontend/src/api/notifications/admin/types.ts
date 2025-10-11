import { z } from 'zod';

export const NotificationSeverity = {
  Info: 1,
  Success: 2,
  Warning: 3,
  Error: 4,
  Critical: 5,
} as const;

export type NotificationSeverity = (typeof NotificationSeverity)[keyof typeof NotificationSeverity];

export const NotificationType = {
  System: 1,
  Booking: 2,
  Payment: 3,
  Account: 4,
  Marketing: 5,
  Reminder: 6,
  Administrative: 7,
  Integration: 8,
} as const;

export type NotificationType = (typeof NotificationType)[keyof typeof NotificationType];

export const adminNotificationDtoSchema = z.object({
  id: z.number(),
  title: z.string(),
  message: z.string(),
  severity: z.union([
    z.literal(NotificationSeverity.Info),
    z.literal(NotificationSeverity.Success),
    z.literal(NotificationSeverity.Warning),
    z.literal(NotificationSeverity.Error),
    z.literal(NotificationSeverity.Critical),
  ]),
  type: z.union([
    z.literal(NotificationType.System),
    z.literal(NotificationType.Booking),
    z.literal(NotificationType.Payment),
    z.literal(NotificationType.Account),
    z.literal(NotificationType.Marketing),
    z.literal(NotificationType.Reminder),
    z.literal(NotificationType.Administrative),
    z.literal(NotificationType.Integration),
  ]),
  relatedEntityId: z.string().nullable(),
  relatedEntityType: z.string().nullable(),
  metadata: z.string().nullable(),
  isRead: z.boolean(),
  createdAt: z.string().datetime(),
  readAt: z.string().datetime().nullable(),
});

export type AdminNotificationDto = z.infer<typeof adminNotificationDtoSchema>;

export const getAdminNotificationsQuerySchema = z.object({
  page: z.number().min(1).default(1),
  pageSize: z.number().min(1).max(100).default(20),
  unreadOnly: z.boolean().optional(),
  severity: z
    .union([
      z.literal(NotificationSeverity.Info),
      z.literal(NotificationSeverity.Success),
      z.literal(NotificationSeverity.Warning),
      z.literal(NotificationSeverity.Error),
      z.literal(NotificationSeverity.Critical),
    ])
    .optional(),
});

export type GetAdminNotificationsQuery = z.infer<typeof getAdminNotificationsQuerySchema>;
