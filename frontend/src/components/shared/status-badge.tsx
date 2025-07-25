import { Badge } from '@/components/ui/badge';
import { cn } from '@/utils/cn';

export type UserStatus = 'online' | 'offline' | 'away' | 'busy' | 'available' | 'mentor' | 'student';

interface StatusBadgeProps {
  status: UserStatus;
  className?: string;
  showIcon?: boolean;
}

const statusConfig = {
  online: {
    label: 'Online',
    variant: 'default' as const,
    color: 'bg-green-500',
    icon: '🟢',
  },
  offline: {
    label: 'Offline',
    variant: 'secondary' as const,
    color: 'bg-gray-400',
    icon: '⚫',
  },
  away: {
    label: 'Away',
    variant: 'secondary' as const,
    color: 'bg-yellow-500',
    icon: '🟡',
  },
  busy: {
    label: 'Busy',
    variant: 'destructive' as const,
    color: 'bg-red-500',
    icon: '🔴',
  },
  available: {
    label: 'Available',
    variant: 'default' as const,
    color: 'bg-green-500',
    icon: '✅',
  },
  mentor: {
    label: 'Mentor',
    variant: 'default' as const,
    color: 'bg-blue-500',
    icon: '👨‍🏫',
  },
  student: {
    label: 'Student',
    variant: 'outline' as const,
    color: 'bg-purple-500',
    icon: '👨‍🎓',
  },
};

export function StatusBadge({ 
  status, 
  className = '', 
  showIcon = false 
}: StatusBadgeProps) {
  const config = statusConfig[status];
  
  return (
    <Badge 
      variant={config.variant} 
      className={cn('text-xs', className)}
    >
      {showIcon && <span className="mr-1">{config.icon}</span>}
      {config.label}
    </Badge>
  );
}

// Online status indicator (just the dot)
export function OnlineStatusDot({ 
  status, 
  className = '' 
}: { status: UserStatus; className?: string }) {
  const config = statusConfig[status];
  
  return (
    <div 
      className={cn(
        'w-3 h-3 rounded-full border-2 border-background',
        config.color,
        className
      )}
      title={config.label}
    />
  );
}