import { Clock, Save, RotateCcw } from 'lucide-react';
import { Button, Card, CardContent } from '@/components/ui';

interface ScheduleActionsProps {
  hasChanges: boolean;
  isSaving: boolean;
  onSave: () => void;
  onReset: () => void;
}

export function ScheduleActions({
  hasChanges,
  isSaving,
  onSave,
  onReset,
}: ScheduleActionsProps) {
  return (
    <Card className="mt-8">
      <CardContent>
        <div className="flex items-center justify-between">
          <div className="text-sm text-gray-600">
            {hasChanges ? 'You have unsaved changes' : 'All changes saved'}
          </div>
          <div className="flex items-center gap-3">
            <Button variant="outline" onClick={onReset} disabled={!hasChanges}>
              <RotateCcw className="w-4 h-4 mr-2" />
              Reset
            </Button>
            <Button
              onClick={onSave}
              disabled={!hasChanges || isSaving}
              className="min-w-24"
            >
              {isSaving ? (
                <>
                  <Clock className="w-4 h-4 mr-2 animate-spin" />
                  Saving...
                </>
              ) : (
                <>
                  <Save className="w-4 h-4 mr-2" />
                  Save
                </>
              )}
            </Button>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
