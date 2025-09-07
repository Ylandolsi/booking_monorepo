import { useState } from 'react';

export const useTimeFilter = () => {
  const [upToDate, setUpToDate] = useState<Date | undefined>(undefined);
  const [timeFilter, setTimeFilter] = useState<TimeFilter>('All');

  const setTimeStatus = (timeStatus: TimeFilter) => {
    setTimeFilter(timeStatus);
    switch (timeStatus) {
      case 'LastHour':
        return setUpToDate(new Date(new Date().getTime() - 60 * 60 * 1000));
      case 'Last24Hours':
        return setUpToDate(new Date(new Date().getTime() - 24 * 60 * 60 * 1000));
      case 'Last3Days':
        return setUpToDate(new Date(new Date().getTime() - 3 * 24 * 60 * 60 * 1000));
      case 'NextHour':
        return setUpToDate(new Date(new Date().getTime() + 60 * 60 * 1000));
      case 'Next24Hours':
        return setUpToDate(new Date(new Date().getTime() + 24 * 60 * 60 * 1000));
      case 'Next3Days':
        return setUpToDate(new Date(new Date().getTime() + 3 * 24 * 60 * 60 * 1000));
      case 'All':
        return setUpToDate(undefined);
    }
  };

  return {
    upToDate,
    setUpToDate,

    timeFilter,
    setTimeFilter,

    setTimeStatus,
  };
};

export type TimeFilter = 'Last24Hours' | 'LastHour' | 'Last3Days' | 'All' | 'Next24Hours' | 'NextHour' | `Next3Days`;
