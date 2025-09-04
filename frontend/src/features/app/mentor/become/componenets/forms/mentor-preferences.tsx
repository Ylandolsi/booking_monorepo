import {
  Alert,
  AlertDescription,
  alertIconMap,
  AlertTitle,
  Button,
  Input,
  Label,
} from '@/components';
import { useBecomeMentor } from '@/features/app/mentor/become/api';
import {
  mentorCreateFormSchema,
  type MentorCreateFormData,
} from '@/features/app/mentor/become/schemas';
import type { Mentor } from '@/features/app/mentor/become/types';
import { zodResolver } from '@hookform/resolvers/zod';
import React from 'react';
import { useForm } from 'react-hook-form';

export function MentorPreferences() {
  const becomeMentorMutation = useBecomeMentor();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<MentorCreateFormData>({
    resolver: zodResolver(mentorCreateFormSchema),
    defaultValues: {},
  });

  const onSubmit = async (data: MentorCreateFormData) => {
    try {
      console.log('mentor');
      await becomeMentorMutation.mutateAsync({
        mentor: {
          hourlyRate: data.hourlyRate,
          bufferTimeMinutes: data.bufferTimeMinutes,
        } as Mentor,
      });
      reset();
    } catch (error) {
      console.error('Failed to become mentor:', error);
    }
  };

  return (
    <>
      <div className="space-y-6">
        <Alert variant={'success'}>
          {React.createElement(alertIconMap['success'])}
          <AlertDescription>
            Great! Your profile is complete. Now set your mentoring preferences.
          </AlertDescription>
        </Alert>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div className="flex flex-col max-w-120  md:flex-row gap-5">
            <div className="space-y-2">
              <Label htmlFor="hourlyRate" className="text-sm font-medium">
                Hourly Rate (USD) *
              </Label>
              <Input
                id="hourlyRate"
                step="1"
                min="1"
                type="number"
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
              <Label
                htmlFor="bufferTimeMinutes"
                className="text-sm font-medium"
              >
                Buffer Time (minutes) *
              </Label>
              <Input
                id="bufferTimeMinutes"
                min="0"
                type="number"
                placeholder="15"
                {...register('bufferTimeMinutes')}
                className={errors.bufferTimeMinutes ? 'border-red-500' : ''}
              />
              {errors.bufferTimeMinutes && (
                <p className="text-sm text-red-600">
                  {errors.bufferTimeMinutes.message}
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
      {becomeMentorMutation.isError && (
        <Alert variant={'destructive'}>
          {React.createElement(alertIconMap['destructive'])}
          <AlertTitle>Error</AlertTitle>
          <AlertDescription>
            {becomeMentorMutation.error.message}
          </AlertDescription>
        </Alert>
      )}

      {becomeMentorMutation.isSuccess && (
        <Alert variant={'success'}>
          {React.createElement(alertIconMap['success'])}
          <AlertTitle>Error</AlertTitle>
          <AlertDescription>Registed successfully</AlertDescription>
        </Alert>
      )}
    </>
  );
}
