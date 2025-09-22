import { Copy } from 'lucide-react';
import { Button, Card, CardContent } from '@/components/ui';
import {
  DAYS_OF_WEEK,
  type DayOfWeek,
} from '@/features/app/session/booking/shared';

interface CopyModeAlertProps {
  selectedCopySource: DayOfWeek;
  onCancel: () => void;
}

export function CopyModeAlert({
  selectedCopySource,
  onCancel,
}: CopyModeAlertProps) {
  const sourceDayLabel = DAYS_OF_WEEK.find(
    (d) => d.key === selectedCopySource,
  )?.label;

  return (
    <Card className="mb-6 border-orange-200 bg-orange-50">
      <CardContent>
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-2">
            <Copy className="w-4 h-4 text-orange-600" />
            <span className="text-orange-800 font-medium">
              Copy mode: Select days to copy availability from {sourceDayLabel}
            </span>
          </div>
          <Button variant="outline" size="sm" onClick={onCancel}>
            Cancel
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}
