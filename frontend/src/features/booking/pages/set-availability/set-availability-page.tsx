import {
  DAYS_OF_WEEK,
  CopyModeAlert,
  SaveSuccessAlert,
  ScheduleActions,
  useAvailabilitySchedule,
  DayAvailability,
  SummaryCard,
} from '@/features/booking/pages';

export function SetAvailabilityPage() {
  const {
    schedule,
    hasChanges,
    isSaving,
    saveSuccess,
    selectedCopySource,
    actions,
    getScheduleSummary,
  } = useAvailabilitySchedule();

  const summary = getScheduleSummary();

  return (
    <div className="container mx-auto py-10 px-4 max-w-6xl">
      <h3 className="text-2xl font-bold text-gray-900 mb-4 text-left">
        Set Your Availability
      </h3>

      <SummaryCard summary={summary} />

      <SaveSuccessAlert show={saveSuccess} />

      {selectedCopySource && (
        <CopyModeAlert
          selectedCopySource={selectedCopySource}
          onCancel={() => actions.setSelectedCopySource(null)}
        />
      )}

      <div className="space-y-6">
        {DAYS_OF_WEEK.map(({ key, label }) => {
          const daySchedule = schedule.find((s) => s.day === key)!;
          return (
            <DayAvailability
              key={key}
              keyWeek={key}
              timeRanges={daySchedule.timeRanges}
              isEnabled={daySchedule.enabled}
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

      <ScheduleActions
        hasChanges={hasChanges}
        isSaving={isSaving}
        onSave={actions.saveAvailability}
        onReset={actions.resetChanges}
      />
    </div>
  );
}
