// --- Monthly Availability Advanced Tests ---

// GetMentorAvailabilityByMonth_ShouldReturnCorrectSlots_WithComplexSchedule
// When Mentor has a complex weekly schedule with split days, Should return correct number of slots per day
// and ensure split days (like Monday) are handled correctly.

// GetMentorAvailabilityByMonth_ShouldHandleLeapYear_February
// When Mentor is available in February of a leap year, Should return 29 days for February.

// GetMentorAvailabilityByMonth_ShouldFilterBookedSlots_WhenIncludeBookedSlotsIsFalse
// When Booked slots exist and includeBookedSlots=false, Should exclude booked slots from availability.

// GetMentorAvailabilityByMonth_ShouldHandleInvalidMonth_ReturnBadRequest
// When Month parameter is invalid (e.g., 13), Should return BadRequest (400).


// --- Daily Availability Advanced Tests ---

// GetMentorAvailabilityByDay_ShouldCalculateSlots_WithBufferTime
// When Mentor has buffer time set, Should calculate slots spaced according to buffer time and return full availability initially.

// GetMentorAvailabilityByDay_ShouldHandleOverlappingSessions_WithBufferTime
// When Multiple overlapping sessions exist with buffer time, Should mark overlapping and buffer slots as unavailable.

// GetMentorAvailabilityByDay_ShouldHandlePastDate_ReturnEmptyOrError
// When Requested date is in the past, Should return empty or special handling (depending on business logic).

// GetMentorAvailabilityByDay_ShouldHandleNonExistentMentor_ReturnNotFound
// When Mentor slug does not exist, Should return NotFound (404).


// --- Edge Cases and Error Scenarios ---

// AvailabilityEndpoints_ShouldHandleConcurrentRequests_Gracefully
// When Multiple concurrent requests are made to the availability endpoint, Should handle them without errors.

// AvailabilityEndpoints_ShouldHandleSpecialCharacters_InMentorSlug
// When Mentor slug contains special characters, Should still return correct availability with same slug in response.

// AvailabilityEndpoints_ShouldHandleTimeZoneBoundaries_Correctly
// When Availability covers almost full day across timezone boundaries, Should correctly generate all slots.

// GetMentorAvailabilityByMonth_ShouldReturnEmptyDays_ForInactiveMentor
// When Mentor is inactive, Should return empty availability or NotFound (depending on business logic).


// --- Performance and Load Tests ---

// AvailabilityEndpoints_ShouldHandleLargeMonth_WithManySlots
// When Mentor has availability every day with multiple slots, Should handle large number of slots without performance issues.


// --- Business Logic Validation ---

// AvailabilitySlots_ShouldRespectMinimumDuration_Requirements
// When Slot length is exactly minimum allowed duration (e.g., 30 mins), Should still return that slot.

// AvailabilityCalculation_ShouldHandleBufferTimeCorrectly_BetweenSessions
// When Session is booked, Should mark buffer-before and buffer-after slots as unavailable according to buffer time.


// --- BufferTimeAvailabilityTests ---

// Availability_ShouldRespectBufferTime_WhenMentorHasSessions
// When Mentor has buffer time configured but no sessions booked, Should return all slots available initially.

// Availability_ShouldShowBufferTimeAsUnavailable_WhenSessionIsBooked
// When Session is booked and buffer time applies, Should mark adjacent slots within buffer as unavailable.

// Availability_ShouldHandleMultipleSessions_WithBufferTime
// When Multiple sessions exist and buffer time is short, Should still calculate correct available slots.

// Availability_ShouldHandleZeroBufferTime_WhenMentorSetsNoBuffer
// When Mentor sets zero buffer time, Should mark all slots as available.
