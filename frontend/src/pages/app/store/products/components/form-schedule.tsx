import type { UseFormReturn } from 'react-hook-form';
import { FormField, FormItem, FormMessage } from '@/components/ui/form';
import { Button } from '@/components/ui/button';
import { Switch } from '@/components/ui/switch';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { useFormSchedule } from '@/pages/app/store/products/hooks/use-form-schedule';
import { Clock, Timer, X } from 'lucide-react';
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
  Alert,
  AlertDescription,
  AlertTitle,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui';
import type { ProductFormData } from '@/pages/app/store/products';
import { DAYS, TIME_OPTIONS } from '@/api/stores/produtcs/sessions/public/availabilities/shared-booking-type';

export function FormScheduleComponent({ form }: { form: UseFormReturn<ProductFormData> }) {
  const { schedule, actions, error } = useFormSchedule(form);

  return (
    <Accordion type="single" collapsible className="w-full">
      <AccordionItem value="social-links">
        <AccordionTrigger className="text-foreground hover:text-primary text-md">
          <div className="flex items-start justify-center gap-3">
            <Timer />
            Setup Schedule
          </div>
        </AccordionTrigger>
        <AccordionContent className="space-y-4">
          {error && (
            <Alert variant={'destructive'} className="">
              <AlertTitle>Error</AlertTitle>
              <AlertDescription>{error}</AlertDescription>
            </Alert>
          )}
          {DAYS.map((day) => {
            const daySchedule = schedule.find((s) => s.dayOfWeek === day);
            if (!daySchedule) return null;

            return (
              <Card key={day} className={`cursor-pointer`}>
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
                      <div key={range.id} className="bg-accent card flex items-center gap-3 rounded-lg border p-2">
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
        </AccordionContent>
        {/* Form Validation Error */}
        <FormField
          control={form.control}
          name="dayAvailabilities"
          render={() => (
            <FormItem>
              <FormMessage />
            </FormItem>
          )}
        />
      </AccordionItem>
    </Accordion>
  );
}
