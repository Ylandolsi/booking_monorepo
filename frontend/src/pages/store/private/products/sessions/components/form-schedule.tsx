import type { UseFormReturn } from 'react-hook-form';
import { FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Button } from '@/components/ui/button';
import { Switch } from '@/components/ui/switch';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { useFormSchedule } from '@/pages/store/private/products/hooks/use-form-schedule';
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
  Textarea,
} from '@/components/ui';
import type { ProductFormData } from '@/pages/store/private/products';
import { DAYS, TIME_OPTIONS } from '@/api/stores/produtcs/sessions/public/availabilities/shared-booking-type';

export function FormSession({ form }: { form: UseFormReturn<ProductFormData> }) {
  return (
    <div className="space-y-4">
      {/* Duration */}
      <div className="flex w-full flex-wrap gap-4">
        <FormField
          control={form.control}
          name="durationMinutes"
          render={({ field }) => (
            <FormItem className="flex-1">
              <FormLabel className="text-foreground">Duration (minutes) *</FormLabel>
              <Select onValueChange={(value) => field.onChange(Number(value))} value={field.value?.toString()}>
                <FormControl>
                  <SelectTrigger className="w-full">
                    <SelectValue placeholder="Select duration" />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  <SelectItem value="30">30</SelectItem>
                  {/* only 30 minutes available for now */}
                </SelectContent>
              </Select>
              <FormMessage />
            </FormItem>
          )}
        />

        {/* Buffer Time */}
        <FormField
          control={form.control}
          name="bufferTimeMinutes"
          render={({ field }) => (
            <FormItem className="flex-1">
              <FormLabel className="text-foreground">Buffer Time (minutes)</FormLabel>
              <FormControl>
                <Select onValueChange={(value) => field.onChange(Number(value))} value={field.value?.toString()}>
                  <FormControl>
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Select duration" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    <SelectItem value="15">15</SelectItem>
                    <SelectItem value="30">30</SelectItem>
                    <SelectItem value="45">45</SelectItem>
                    {/* only 30 minutes available for now */}
                  </SelectContent>
                </Select>
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
      </div>

      {/* Meeting Instructions */}
      <FormField
        control={form.control}
        name="meetingInstructions"
        render={({ field }) => (
          <FormItem>
            <FormLabel className="text-foreground">Meeting Instructions</FormLabel>
            <FormControl>
              <Textarea placeholder="What should customers know before the meeting?" rows={3} className="resize-none" {...field} />
            </FormControl>
            <FormMessage />
          </FormItem>
        )}
      />

      <FormScheduleComponent form={form} />
    </div>
  );
}

function FormScheduleComponent({ form }: { form: UseFormReturn<ProductFormData> }) {
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
