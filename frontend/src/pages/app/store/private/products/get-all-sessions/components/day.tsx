import React, { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { type DailySessions } from '@/api/stores/produtcs/sessions/private/get-all-sessions';

export interface DayProps {
  classNames: string;
  day: DailySessions & { classNames: string | undefined };
  onHover: (day: number | null) => void;
}

export const Day: React.FC<DayProps> = ({ classNames, day, onHover }) => {
  const [isHovered, setIsHovered] = useState(false);

  return (
    <>
      <motion.div
        className={`bg-primary relative flex items-center justify-center py-1 ${classNames}`}
        style={{ height: '4rem', borderRadius: 16 }}
        onMouseEnter={() => {
          setIsHovered(true);
          onHover(day.day);
        }}
        onMouseLeave={() => {
          setIsHovered(false);
          onHover(null);
        }}
        id={`day-${day.day}`}
      >
        <motion.div className="flex flex-col items-center justify-center">
          {/* {!(day.day[0] === '+' || day.day[0] === '-') && <span className="text-primary-foreground text-sm">{day.day}</span>} */}
          <span className="text-sm">{day.day}</span>{' '}
        </motion.div>
        {day.sessions && (
          <motion.div
            className="bg-accent text-accent-foreground absolute right-1 bottom-1 flex size-5 items-center justify-center rounded-full p-1 text-[10px] font-bold"
            layoutId={`day-${day.day}-meeting-count`}
            style={{
              borderRadius: 999,
            }}
          >
            {day.sessions.length}
          </motion.div>
        )}

        <AnimatePresence>
          {day.sessions && isHovered && (
            <div className="absolute inset-0 flex size-full items-center justify-center">
              <motion.div
                className="bg-accent text-accent-foreground flex size-10 items-center justify-center p-1 text-xs font-bold"
                layoutId={`day-${day.day}-meeting-count`}
                style={{
                  borderRadius: 999,
                }}
              >
                {day.sessions.length}
              </motion.div>
            </div>
          )}
        </AnimatePresence>
      </motion.div>
    </>
  );
};
