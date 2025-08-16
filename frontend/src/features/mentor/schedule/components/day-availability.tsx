import { Card, CardHeader, CardTitle, Button, CardContent } from '@/components';

import { formatTimeRange } from '@/utils';
import {
  Separator,
  Switch,
  Select,
  SelectTrigger,
  SelectValue,
  SelectContent,
  SelectItem,
  Badge,
} from '@/components/ui';
import { Copy, Clock, X, Plus } from 'lucide-react';
import {
  PREDEFINED_TIME_SLOTS,
  TIME_OPTIONS,
  type DayOfWeek,
} from '@/features/booking/shared';
import type { AvailabilityRangeType } from '@/features/mentor/schedule/types';

interface DayAvailabilityProps {
  keyWeek: DayOfWeek;
  timeRanges: AvailabilityRangeType[];
  isEnabled: boolean;
  selectedCopySource: DayOfWeek | null;
  setSelectedCopySource: (day: DayOfWeek | null) => void;
  toggleDay: (day: DayOfWeek) => void;
  addPredefinedTimeSlot: (
    day: DayOfWeek,
    timeSlot: { startTime: string; endTime: string },
  ) => void;
  addCustomTimeSlot: (day: DayOfWeek) => void;
  updateTimeRange: (
    day: DayOfWeek,
    rangeId: string,
    field: 'startTime' | 'endTime',
    value: string,
  ) => void;
  removeTimeRange: (day: DayOfWeek, rangeId: string) => void;
  copyAvailability: (fromDay: DayOfWeek, toDay: DayOfWeek) => void;
  label: string;
}

export function DayAvailability(props: DayAvailabilityProps) {
  return (
    <Card
      className={`transition-all ${props.isEnabled ? 'border-blue-200 bg-blue-50/30' : ''}`}
    >
      <CardHeader>
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <Switch
              checked={props.isEnabled}
              onCheckedChange={() => props.toggleDay(props.keyWeek)}
            />
            <div>
              <CardTitle className="text-lg">{props.label}</CardTitle>
              <p className="text-sm text-gray-600 mt-1">
                {props.isEnabled
                  ? `${props.timeRanges.length} time slot${props.timeRanges.length !== 1 ? 's' : ''} configured`
                  : 'Not available'}
              </p>
            </div>
          </div>

          {props.isEnabled && (
            <div className="flex items-center gap-2">
              {props.timeRanges.length > 0 && (
                <Button
                  variant="outline"
                  size="sm"
                  onClick={() =>
                    props.setSelectedCopySource(
                      props.selectedCopySource === props.keyWeek
                        ? null
                        : props.keyWeek,
                    )
                  }
                  className={
                    props.selectedCopySource === props.keyWeek
                      ? 'bg-orange-100 border-orange-300'
                      : ''
                  }
                >
                  <Copy className="w-4 h-4 mr-2" />
                  {props.selectedCopySource === props.keyWeek
                    ? 'Cancel Copy'
                    : 'Copy'}
                </Button>
              )}

              {props.selectedCopySource &&
                props.selectedCopySource !== props.keyWeek && (
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() =>
                      props.copyAvailability(
                        props.selectedCopySource!,
                        props.keyWeek,
                      )
                    }
                  >
                    Paste Here
                  </Button>
                )}
            </div>
          )}
        </div>
      </CardHeader>

      {props.isEnabled && (
        <CardContent>
          {/* Time Ranges */}
          <div className="space-y-3 mb-4">
            {props.timeRanges.map((range) => (
              <div
                key={range.id}
                className="flex items-center gap-3 p-3 bg-white rounded-lg border"
              >
                <Clock className="w-4 h-4 text-gray-500" />

                <Select
                  value={range.startTime}
                  onValueChange={(value) =>
                    props.updateTimeRange(
                      props.keyWeek,
                      range.id?.toString() || '0',
                      'startTime',
                      value,
                    )
                  }
                >
                  <SelectTrigger className="w-24">
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    {TIME_OPTIONS.map((time) => (
                      <SelectItem key={time} value={time}>
                        {time}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>

                <span className="text-gray-500">to</span>

                <Select
                  value={range.endTime}
                  onValueChange={(value) =>
                    props.updateTimeRange(
                      props.keyWeek,
                      range.id?.toString() || '',
                      'endTime',
                      value,
                    )
                  }
                >
                  <SelectTrigger className="w-24">
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    {TIME_OPTIONS.map((time) => (
                      <SelectItem key={time} value={time}>
                        {time}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>

                <Badge variant="secondary" className="ml-auto">
                  {formatTimeRange(range.startTime, range.endTime)}
                </Badge>

                <Button
                  variant="ghost"
                  size="sm"
                  onClick={() =>
                    props.removeTimeRange(
                      props.keyWeek,
                      range.id?.toString() || '',
                    )
                  }
                  className="text-red-600 hover:text-red-700 hover:bg-red-50"
                >
                  <X className="w-4 h-4" />
                </Button>
              </div>
            ))}
          </div>

          {/* Add Time Slot Options */}
          <div className="space-y-3">
            <Separator />

            {/* Predefined Time Slots */}
            <div>
              <h4 className="text-sm font-medium text-gray-700 mb-2">
                Quick Add:
              </h4>
              <div className="flex flex-wrap gap-2">
                {PREDEFINED_TIME_SLOTS.map((slot) => {
                  const alreadyExists = props.timeRanges.some(
                    (range) =>
                      range.startTime <= slot.start &&
                      range.endTime >= slot.end,
                  );
                  return (
                    <Button
                      key={`${slot.start}-${slot.end}`}
                      variant="outline"
                      size="sm"
                      disabled={alreadyExists}
                      onClick={() =>
                        props.addPredefinedTimeSlot(props.keyWeek, {
                          startTime: slot.start,
                          endTime: slot.end,
                        })
                      }
                      className="text-xs"
                    >
                      {slot.label}
                    </Button>
                  );
                })}
              </div>
            </div>

            {/* Custom Time Slot */}
            <Button
              variant="outline"
              size="sm"
              onClick={() => props.addCustomTimeSlot(props.keyWeek)}
              className="w-full"
            >
              <Plus className="w-4 h-4 mr-2" />
              Add Custom Time Slot
            </Button>
          </div>
        </CardContent>
      )}
    </Card>
  );
}
