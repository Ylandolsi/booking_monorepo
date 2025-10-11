import React, { useMemo, useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { ArrowLeft, ArrowRight } from 'lucide-react';
import { useAllSessionMonthly, type DailySessions, type SessionResponse } from '@/api/stores/produtcs/sessions/private/get-all-sessions';
import { ErrorComponenet, InputToCopy, LoadingState } from '@/components';
import { DeepCopy, GenerateTimeZoneId } from '@/lib';

import { Day, type DayProps } from '@/pages/store/private/products/sessions/get-all-sessions/components/day';

const CalendarGrid: React.FC<{ onHover: (day: number | null) => void; days: DayProps['day'][] }> = ({ onHover, days }) => {
  return (
    <div className="grid grid-cols-7 gap-2">
      {days.map((day, index) => (
        <Day key={`${day.day}-${index}`} classNames={day?.classNames ?? ''} day={day} onHover={onHover} />
      ))}
    </div>
  );
};

const InteractiveCalendar = React.forwardRef<HTMLDivElement, React.HTMLAttributes<HTMLDivElement>>(({ className, ...props }, ref) => {
  const [hoveredDay, setHoveredDay] = useState<number | null>(null);
  const currentDate = new Date();
  const [date, setDate] = useState<Date>(new Date(currentDate.getFullYear(), currentDate.getMonth(), 1)); // first day of current month
  const [meetingLink, setMeetingLink] = useState<string>('');
  const year = date.getFullYear();
  const month = date.getMonth(); // 0-indexed (0 = January, 11 = December)
  const timeZoneId = GenerateTimeZoneId();
  const { data, isLoading, error } = useAllSessionMonthly({ year: year, month: month + 1, timeZoneId: timeZoneId });
  const firstDayOfWeek = date.getDay(); // 0 (Sun) to 6 (Sat)

  const days = useMemo(() => {
    let days = DeepCopy(data?.days) || ([] as DailySessions[]);
    if (!days) return [];
    for (let i = 0; i < firstDayOfWeek; i++) {
      days.unshift({ day: '', classNames: 'bg-transparent' });
    }

    for (let i = 0; i < days?.length; i++) {
      if (days[i]?.sessions?.length <= 0 || !days[i]?.sessions) {
        days[i].classNames = 'bg-zinc-700/20';
      } else {
        days[i].classNames = ' text-background cursor-pointer';
      }
    }
    return days;
  }, [data, firstDayOfWeek]);

  const filteredDays = useMemo(() => (hoveredDay && days ? [...days].filter((day) => day.day === hoveredDay) : days), [days, hoveredDay]);

  if (isLoading) {
    return <LoadingState type="spinner" message="Loading calendar..." />;
  }
  if (error) {
    return <ErrorComponenet title="Failed to load calendar" message="An error occurred while fetching calendar data." />;
  }

  const handleDayHover = (day: number | null) => {
    setHoveredDay(day);
  };

  const formatDateToMonthYear = (date: Date) => {
    return new Intl.DateTimeFormat('en-US', { month: 'short', year: 'numeric' }).format(date);
  };

  const nextMonth = () => {
    setDate((prevDate) => {
      const newDate = new Date(prevDate);
      newDate.setMonth(newDate.getMonth() + 1);
      return newDate;
    });
  };

  const prevMonth = () => {
    setDate((prevDate) => {
      const newDate = new Date(prevDate);
      newDate.setMonth(newDate.getMonth() - 1);
      return newDate;
    });
  };

  return (
    <AnimatePresence mode="wait">
      {/* @ts-ignore */}
      <motion.div ref={ref} className="relative mx-auto my-10 flex w-full flex-col items-center justify-center gap-8 lg:flex-row" {...props}>
        <motion.div layout className="w-full max-w-lg">
          <motion.div key="calendar-view" className="flex w-full flex-col gap-4">
            <div className="flex w-full items-center justify-between gap-2">
              <div className="flex items-center justify-center" onClick={prevMonth}>
                <ArrowLeft className="h-6 w-6" />
              </div>
              <motion.h2 className="mb-2 text-4xl font-bold tracking-wider">{formatDateToMonthYear(date)}</motion.h2>
              <div className="flex items-center justify-center" onClick={nextMonth}>
                <ArrowRight className="h-6 w-6" />
              </div>
            </div>
            <div className="grid grid-cols-7 gap-2">
              {daysOfWeek.map((day) => (
                <div key={day} className="px-0/5 bg-primary text-background rounded-xl py-1 text-center text-xs">
                  {day}
                </div>
              ))}
            </div>
            <CalendarGrid onHover={handleDayHover} days={days} />
          </motion.div>
        </motion.div>
        {
          <motion.div
            className="w-full max-w-lg"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: 20 }}
            transition={{ duration: 0.3 }}
          >
            <motion.div key="more-view" className="mt-4 flex w-full flex-col gap-4">
              <div className="flex w-full flex-col items-start justify-between">
                <motion.h2 className="mb-2 text-4xl font-bold tracking-wider">Bookings</motion.h2>
                <p className="font-medium">See upcoming and past events booked through your event type links.</p>
              </div>
              <motion.div
                className="border-border bg-card flex h-[620px] flex-col items-start justify-start overflow-hidden overflow-y-scroll rounded-xl border shadow-xl"
                layout
              >
                <motion.div className={`border-border w-full border-b-1 p-3`} layout>
                  <InputToCopy label="Meeting Link" input={meetingLink} className="mb-4" />
                </motion.div>
                <AnimatePresence>
                  {filteredDays
                    .filter((day: DailySessions) => day.sessions !== undefined && day.sessions.length > 0)
                    .map((day: DailySessions) => (
                      <motion.div key={day.day} className={`border-border w-full border-b-1 py-0 last:border-b-0`} layout>
                        {day.sessions &&
                          day.sessions.map((meeting: SessionResponse, mIndex: number) => (
                            <motion.div
                              key={mIndex}
                              className="cursor-pointer border-b border-[#323232] p-3 last:border-b-0"
                              initial={{ opacity: 0, y: 10 }}
                              animate={{ opacity: 1, y: 0 }}
                              exit={{ opacity: 0, y: -10 }}
                              transition={{
                                duration: 0.2,
                                delay: mIndex * 0.05,
                              }}
                              onClick={() => setMeetingLink(meeting.googleMeetLink || '')}
                            >
                              <div className="mb-2 flex items-center justify-between">
                                <span className="text-sm">{meeting.date}</span>
                                <span className="text-sm">{meeting.time}</span>
                              </div>
                              <h3 className="mb-1 text-lg font-semibold">{meeting.title}</h3>
                              <p className="mb-1 text-sm text-zinc-600">{meeting.participants.join(', ')}</p>
                              <div className="text-primary flex items-center">
                                <svg
                                  className="mr-1 h-4 w-4"
                                  fill="none"
                                  stroke="currentColor"
                                  viewBox="0 0 24 24"
                                  xmlns="http://www.w3.org/2000/svg"
                                >
                                  <path
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                    strokeWidth={2}
                                    d="M15 10l4.553-2.276A1 1 0 0121 8.618v6.764a1 1 0 01-1.447.894L15 14M5 18h8a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v8a2 2 0 002 2z"
                                  />
                                </svg>
                                <span className="text-sm">{meeting.location}</span>
                              </div>
                            </motion.div>
                          ))}
                      </motion.div>
                    ))}
                </AnimatePresence>
              </motion.div>
            </motion.div>
          </motion.div>
        }
      </motion.div>
    </AnimatePresence>
  );
});
InteractiveCalendar.displayName = 'InteractiveCalendar';

export default InteractiveCalendar;

const daysOfWeek = ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT'];
