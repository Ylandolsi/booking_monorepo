import { useUser } from '@/features/auth';
import {
  Checkbox,
  Label,
  Progress,
  Button,
  Input,
  Alert,
  alertIconMap,
  AlertDescription,
  AlertTitle,
} from '@/components/ui';
import { useBecomeMentor } from '@/features/mentor/api';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import React from 'react';
import type { Mentor } from '@/features/mentor/types';

const mentorFormSchema = z.object({
  hourlyRate: z
    .string()
    .min(1, 'Hourly rate is required')
    .refine((val) => !isNaN(Number(val)) && Number(val) > 0, {
      message: 'Hourly rate must be a positive number',
    })
    .transform((val) => Number(val)),
  bufferTime: z
    .string()
    .min(1, 'Buffer time is required')
    .refine((val) => !isNaN(Number(val)) && Number(val) > 0, {
      message: 'Buffer time must be a positive number',
    })
    .transform((val) => Number(val)),
});

type MentorFormData = z.infer<typeof mentorFormSchema>;

export function BecomeMentorPage() {
  const becomeMentorMutation = useBecomeMentor();
  const { data: user } = useUser();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<MentorFormData>({
    resolver: zodResolver(mentorFormSchema),
    defaultValues: {},
  });

  const completion = user?.profileCompletionStatus?.completionStatus ?? 0;
  const total = user?.profileCompletionStatus?.totalFields ?? 1;
  const percentage = Math.floor((completion / total) * 100);
  const isProfileComplete = percentage === 100;

  const onSubmit = async (data: MentorFormData) => {
    try {
      await becomeMentorMutation.mutateAsync({
        mentor: {
          hourlyRate: data.hourlyRate,
          bufferTimeMinutes: data.bufferTime,
        } as Mentor,
      });
      reset();
    } catch (error) {
      console.error('Failed to become mentor:', error);
    }
  };

  const profileFields = Object.entries(
    user?.profileCompletionStatus ?? {},
  ).filter(([key]) => key !== 'completionStatus' && key !== 'totalFields');

  // Format field names for display
  const formatFieldName = (fieldName: string): string => {
    return fieldName
      .replace(/([A-Z])/g, ' $1')
      .replace(/^./, (str) => str.toUpperCase())
      .trim();
  };

  return (
    <div className="py-10 mx-10 space-y-6">
      <div className="space-y-2">
        <h1 className="font-bold text-2xl">Become a Mentor</h1>
        <div className="space-y-2">
          <Label className="text-sm text-gray-600">
            Profile Completion: {percentage}%
          </Label>
          <Progress value={percentage} className="w-full" />
        </div>
      </div>

      {isProfileComplete ? (
        <div className="space-y-6">
          <Alert variant={'success'}>
            {React.createElement(alertIconMap['success'])}
            <AlertDescription>
              Great! Your profile is complete. Now set your mentoring
              preferences.
            </AlertDescription>
          </Alert>
          {/* </div> */}

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
            <div className="grid gap-4 md:grid-cols-2">
              <div className="space-y-2">
                <Label htmlFor="hourlyRate" className="text-sm font-medium">
                  Hourly Rate (TND) *
                </Label>
                <Input
                  id="hourlyRate"
                  step="0.01"
                  min="0"
                  placeholder="20.00"
                  {...register('hourlyRate')}
                  className={errors.hourlyRate ? 'border-red-500' : ''}
                />
                {errors.hourlyRate && (
                  <p className="text-sm text-red-600">
                    {errors.hourlyRate.message}
                  </p>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="bufferTime" className="text-sm font-medium">
                  Buffer Time (minutes) *
                </Label>
                <Input
                  id="bufferTime"
                  min="0"
                  placeholder="15"
                  {...register('bufferTime')}
                  className={errors.bufferTime ? 'border-red-500' : ''}
                />
                {errors.bufferTime && (
                  <p className="text-sm text-red-600">
                    {errors.bufferTime.message}
                  </p>
                )}
                <p className="text-xs text-gray-500">
                  Time between sessions for preparation
                </p>
              </div>
            </div>

            <Button
              type="submit"
              disabled={isSubmitting || becomeMentorMutation.isPending}
              className="w-full md:w-auto"
            >
              {isSubmitting || becomeMentorMutation.isPending
                ? 'Processing...'
                : 'Become Mentor'}
            </Button>
          </form>
        </div>
      ) : (
        <div className="space-y-4">
          <Alert variant={'info'}>
            {React.createElement(alertIconMap['info'])}
            <AlertTitle>Complete your profile to become a mentor</AlertTitle>
            <AlertDescription>
              You need to fill out all required fields before you can offer
              mentoring services.
            </AlertDescription>
          </Alert>

          <div className="grid gap-3 md:grid-cols-2">
            {profileFields.map(([key, isCompleted]) => (
              <div
                key={key}
                className="flex items-center space-x-3 p-3 border rounded-lg"
              >
                <Checkbox
                  id={key}
                  checked={isCompleted === true}
                  disabled
                  className="data-[state=checked]:bg-green-600"
                />
                <Label
                  htmlFor={key}
                  className={`font-medium ${
                    isCompleted ? 'text-green-700' : 'text-gray-600'
                  }`}
                >
                  {formatFieldName(key)}
                </Label>
              </div>
            ))}
          </div>

          <Button
            disabled={true}
            variant="secondary"
            className="w-full md:w-auto opacity-50 cursor-not-allowed"
          >
            Complete Profile First
          </Button>
        </div>
      )}

      {becomeMentorMutation.isError && (
        <Alert variant={'destructive'}>
          {React.createElement(alertIconMap['destructive'])}
          <AlertTitle>Error</AlertTitle>
          <AlertDescription>
            Failed to register as a mentor. Please try again.
          </AlertDescription>
        </Alert>
      )}

      {becomeMentorMutation.isSuccess && (
        <Alert variant={'success'}>
          {React.createElement(alertIconMap['success'])}
          <AlertTitle>Error</AlertTitle>
          <AlertDescription>
            Failed to register as a mentor. Please try again.
          </AlertDescription>
        </Alert>
      )}
    </div>
  );
}
