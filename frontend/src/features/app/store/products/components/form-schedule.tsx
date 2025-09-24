import type { UseFormReturn } from 'react-hook-form';
import { FormField, FormItem, FormMessage } from '@/components/ui/form';
import { Button } from '@/components/ui/button';
import { Switch } from '@/components/ui/switch';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { useFormSchedule } from '@/features/app/store/products/hooks/use-form-schedule';
import { mapDayToNumber } from '@/utils/enum-days-week';
import { TIME_OPTIONS, type DayOfWeek } from '@/features/app/session/booking/shared';
import { Clock, X } from 'lucide-react';
import { Label, Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui';
import type { ProductFormData } from '@/features/app/store/products';

const DAYS: DayOfWeek[] = ['monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday', 'sunday'];

export function FormScheduleComponent({ form }: { form: UseFormReturn<ProductFormData> }) {
  const { schedule, actions } = useFormSchedule(form);

  return (
    <div className="space-y-6">
      <Label> Availability </Label>
      <div className="space-y-4">
        {DAYS.map((day) => {
          const daySchedule = schedule.find((s) => s.dayOfWeek === mapDayToNumber(day));
          if (!daySchedule) return null;

          return (
            <Card key={day} className={`cursor-pointer hover:bg-gray-50`}>
              <CardHeader>
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-4">
                    <Switch className="border-border border" checked={daySchedule.isActive} onCheckedChange={() => actions.toggleDay(day)} />
                    <div>
                      <CardTitle className="text-lg">{day.charAt(0).toUpperCase() + day.slice(1)}</CardTitle>
                      <p className="mt-1 text-sm text-gray-600">
                        {daySchedule.isActive
                          ? `${daySchedule.availabilityRanges.length} time slot${daySchedule.availabilityRanges.length !== 1 ? 's' : ''} configured`
                          : 'Not available'}
                      </p>
                    </div>
                  </div>
                </div>
              </CardHeader>

              {daySchedule.isActive && (
                <CardContent className="space-y-3">
                  {/* Time Ranges */}
                  {daySchedule.availabilityRanges.map((range) => (
                    <div key={range.id} className="flex items-center gap-3 rounded-lg border bg-white p-3">
                      <Clock className="h-4 w-4 text-gray-500" />
                      <Select
                        value={range.startTime}
                        onValueChange={(value) => actions.updateTimeRange(day, range.id?.toString() || '0', 'startTime', value)}
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
                        onValueChange={(value) => actions.updateTimeRange(day, range.id?.toString() || '', 'endTime', value)}
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
                      <Button
                        type="button"
                        variant="ghost"
                        size="sm"
                        onClick={() => actions.removeTimeRange(day, range.id?.toString() || '')}
                        className="ml-auto text-red-600 hover:bg-red-50 hover:text-red-700"
                      >
                        <X className="h-4 w-4" />
                      </Button>
                    </div>
                  ))}

                  {/* Add Time Slot Button */}
                  <Button type="button" variant="outline" size="sm" onClick={() => actions.addCustomTimeSlot(day)} className="w-full">
                    + Add Time Slot
                  </Button>
                </CardContent>
              )}
            </Card>
          );
        })}
      </div>

      {/* Form Validation Error */}
      <FormField
        control={form.control}
        name="dailySchedule"
        render={() => (
          <FormItem>
            <FormMessage />
          </FormItem>
        )}
      />
    </div>
  );
}
