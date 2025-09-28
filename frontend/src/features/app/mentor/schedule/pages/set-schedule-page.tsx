import { QueryStateWrapper } from '@/components/wrappers';
import {
  CopyModeAlert,
  SaveSuccessAlert,
  ScheduleActions,
  useAvailabilitySchedule,
  DayAvailability,
  SummaryCard,
} from '@/features/app/mentor/schedule';
import { mapDayToNumber } from '@/utils/enum-days-week';
import { MentorGuard } from '@/components';
import { DAYS_OF_WEEK } from '@/api/stores/produtcs/sessions/public/availabilities/shared-booking-type';

function ScheduleContent() {
  const { schedule, hasChanges, isSaving, saveSuccess, selectedCopySource, actions, getScheduleSummary } = useAvailabilitySchedule();

  const summary = getScheduleSummary();

  return (
    <div className="container mx-auto max-w-6xl">
      <h3 className="mb-4 text-left text-2xl font-bold text-gray-900">Set Your Availability</h3>

      <SummaryCard summary={summary} />

      <SaveSuccessAlert show={saveSuccess} />

      {selectedCopySource && <CopyModeAlert selectedCopySource={selectedCopySource} onCancel={() => actions.setSelectedCopySource(null)} />}

      <div className="space-y-6">
        {DAYS_OF_WEEK.map(({ key, label }) => {
          const daySchedule = schedule.find((s) => s.dayOfWeek === mapDayToNumber(key));
          if (!daySchedule) return null;

          return (
            <DayAvailability
              key={key}
              keyWeek={key}
              timeRanges={daySchedule.availabilityRanges}
              isEnabled={daySchedule.isActive}
              selectedCopySource={selectedCopySource}
              setSelectedCopySource={actions.setSelectedCopySource}
              toggleDay={actions.toggleDay}
              addPredefinedTimeSlot={actions.addTimeSlot}
              addCustomTimeSlot={actions.addCustomTimeSlot}
              updateTimeRange={actions.updateTimeRange}
              removeTimeRange={actions.removeTimeRange}
              copyAvailability={actions.copyAvailability}
              label={label}
            />
          );
        })}
      </div>

      <ScheduleActions hasChanges={hasChanges} isSaving={isSaving} onSave={actions.saveAvailability} onReset={actions.resetChanges} />
    </div>
  );
}

export function SetSchedulePage() {
  const { scheduleQuery } = useAvailabilitySchedule();

  const queryState = {
    data: scheduleQuery.data,
    isLoading: scheduleQuery.isLoading,
    isError: scheduleQuery.isError,
    error: scheduleQuery.error as Error | null,
    refetch: scheduleQuery.refetch,
  };

  return (
    <MentorGuard>
      <QueryStateWrapper
        query={queryState}
        loadingMessage="Loading your schedule..."
        loadingType="spinner"
        emptyStateMessage="No schedule data available. Let's set up your availability!"
        containerClassName="min-h-screen"
      >
        {() => <ScheduleContent />}
      </QueryStateWrapper>
    </MentorGuard>
  );
}
