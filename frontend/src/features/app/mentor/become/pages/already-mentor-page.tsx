import React, { useState } from 'react';
import {
  useUpdateMentor,
  useUserMentorData,
} from '@/features/app/mentor/become';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { formatDate } from '@/utils';
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Badge,
  Link,
  Input,
  Label,
  Alert,
  AlertDescription,
  DrawerDialog,
} from '@/components/ui';
import { ProfileStateWrapper } from '@/components/wrappers';
import {
  CheckCircle,
  Calendar,
  DollarSign,
  ArrowRight,
  Settings,
  BarChart3,
  MessageCircle,
  Edit3,
  Loader2,
  AlertCircle,
  Save,
} from 'lucide-react';
import { type MentorUpdateFormData } from '@/features/app/mentor/become/schemas';
import { useQueryState } from '@/hooks';
import { mentorUpdateFormSchema } from '@/features/app/mentor/become/schemas/mentor-update-schema';

type CombinedData = {
  user: any;
  mentor: any;
};

function AlreadyMentorContent({ user, mentor }: CombinedData) {
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const updateMentorMutation = useUpdateMentor();

  // Form setup
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
  } = useForm<MentorUpdateFormData>({
    resolver: zodResolver(mentorUpdateFormSchema),
  });

  // Populate form when mentor data loads or dialog opens
  React.useEffect(() => {
    if (mentor && isEditDialogOpen) {
      setValue('hourlyRate', mentor.hourlyRate?.toString() || '');
      setValue('bufferTimeMinutes', mentor.bufferTimeMinutes?.toString() || '');
    }
  }, [mentor, isEditDialogOpen, setValue]);

  const onSubmit = async (data: MentorUpdateFormData) => {
    try {
      console.log(data);
      await updateMentorMutation.mutateAsync({
        hourlyRate: Number(data.hourlyRate),
        bufferTimeMinutes: Number(data.bufferTimeMinutes),
      });
      setIsEditDialogOpen(false);
      reset();
    } catch (error) {
      console.error('Failed to update mentor details:', error);
    }
  };

  const handleDialogClose = () => {
    setIsEditDialogOpen(false);
    reset();
    updateMentorMutation.reset();
  };

  const quickActions = [
    {
      to: '/app' as const,
      icon: <BarChart3 className="w-5 h-5 mr-2" />,
      title: 'Go to Dashboard',
      description: 'Manage your sessions and students',
    },
    {
      to: '/app' as const,
      icon: <Settings className="w-5 h-5 mr-2" />,
      title: 'Mentor Settings',
      description: 'Advanced preferences and settings',
    },
    {
      to: '/app' as const,
      icon: <Calendar className="w-5 h-5 mr-3 text-blue-600" />,
      title: 'Schedule Session',
      description: 'Add new available time slots',
    },
    {
      to: '/app' as const,
      icon: <MessageCircle className="w-5 h-5 mr-3 text-green-600" />,
      title: 'Message Students',
      description: 'Connect with your mentees',
    },
    {
      to: '/app' as const,
      icon: <DollarSign className="w-5 h-5 mr-3 text-purple-600" />,
      title: 'View Earnings',
      description: 'Track your income and payouts',
    },
  ];

  return (
    <div className="container space-y-6 mx-auto max-w-4xl">
      {/* Success notification */}
      {updateMentorMutation.isSuccess && (
        <Alert className="mb-6 border-green-200 bg-green-50">
          <CheckCircle className="w-4 h-4 text-green-600" />
          <AlertDescription className="text-green-800">
            Your mentor profile has been updated successfully!
          </AlertDescription>
        </Alert>
      )}

      {/* Header Section */}
      <div className="text-center space-y-6 mb-8">
        <div className="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center mx-auto">
          <CheckCircle className="w-10 h-10 text-green-600" />
        </div>

        <div className="space-y-2">
          <h1 className="text-3xl font-bold text-gray-900">
            You're Already a Mentor!
          </h1>
          <p className="text-lg text-gray-600 max-w-2xl mx-auto">
            Welcome back, {user?.firstName || 'there'}! You've been successfully
            registered as a mentor since {formatDate(mentor?.createdAt)}.
          </p>
        </div>

        <Badge className="bg-green-100 text-green-800 px-4 py-2">
          Active Mentor
        </Badge>
      </div>

      {/* Current Status Card with Edit Dialog */}
      <DrawerDialog
        open={isEditDialogOpen}
        onOpenChange={setIsEditDialogOpen}
        title="Update Mentor Profile"
        description="Update your mentoring preferences. These changes will apply to your future sessions."
      >
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="hourlyRate">Hourly Rate ($) *</Label>
            <Input
              id="hourlyRate"
              type="number"
              step="0.01"
              min="0"
              placeholder="45.00"
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
            <Label htmlFor="bufferTimeMinutes">Buffer Time (minutes) *</Label>
            <Input
              id="bufferTimeMinutes"
              type="number"
              min="0"
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

          {updateMentorMutation.isError && (
            <Alert className="border-red-200 bg-red-50">
              <AlertCircle className="w-4 h-4 text-red-600" />
              <AlertDescription className="text-red-800">
                Failed to update profile. Please try again.
              </AlertDescription>
            </Alert>
          )}

          <div className="flex items-center justify-end gap-4">
            <Button
              type="button"
              variant="outline"
              onClick={handleDialogClose}
              disabled={isSubmitting || updateMentorMutation.isPending}
            >
              Cancel
            </Button>
            <Button
              type="submit"
              disabled={isSubmitting || updateMentorMutation.isPending}
            >
              {isSubmitting || updateMentorMutation.isPending ? (
                <>
                  <Loader2 className="w-4 h-4 mr-2 animate-spin" />
                  Updating...
                </>
              ) : (
                <>
                  <Save className="w-4 h-4 mr-2" />
                  Save Changes
                </>
              )}
            </Button>
          </div>
        </form>
      </DrawerDialog>

      <Card>
        <CardContent className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="space-y-3">
              <div className="flex justify-between">
                <span className="font-medium text-gray-600">Hourly Rate:</span>
                <span className="font-semibold">{mentor?.hourlyRate} $</span>
              </div>
              <div className="flex justify-between">
                <span className="font-medium text-gray-600">Buffer Time:</span>
                <span className="font-semibold">
                  {mentor?.bufferTimeMinutes} minutes
                </span>
              </div>
            </div>

            <div className="space-y-3">
              <div className="flex justify-between">
                <span className="font-medium text-gray-600">Member Since:</span>
                <span className="font-semibold">
                  {formatDate(mentor?.createdAt)}
                </span>
              </div>
              <div className="flex justify-between">
                <span className="font-medium text-gray-600">Status:</span>
                <Badge
                  variant="outline"
                  className="text-green-700 border-green-300"
                >
                  Active
                </Badge>
              </div>
            </div>
          </div>
          <div className="flex justify-end">
            <Button
              onClick={() => setIsEditDialogOpen(true)}
              variant="outline"
              size="sm"
            >
              <Edit3 className="w-4 h-4 mr-2" />
              Edit Profile
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Quick Actions */}
      <Card>
        <CardHeader>
          <CardTitle className="text-lg">Quick Actions</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {quickActions.map((action, index) => (
              <Button
                key={index}
                // key={action.to}
                variant="ghost"
                size="lg"
                className="flex h-auto p-4 text-left justify-start"
              >
                <Link to={action.to} className="w-full flex items-center">
                  {action.icon}
                  <div className="text-left">
                    <div className="font-medium">{action.title}</div>
                    <div className="text-xs text-gray-600">
                      {action.description}
                    </div>
                  </div>
                  <ArrowRight className="w-4 h-4 ml-auto" />
                </Link>
              </Button>
            ))}
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

export function AlreadyMentorPage() {
  const combinedQuery = useUserMentorData();

  // Create a properly typed query state for ProfileStateWrapper
  const queryState = useQueryState(
    { user: combinedQuery.user, mentor: combinedQuery.mentor },
    combinedQuery.isLoading || false,
    combinedQuery.isError,
    combinedQuery.error as Error | null,
    combinedQuery.refetch,
  );

  return (
    <ProfileStateWrapper
      query={queryState}
      requiresMentor={true}
      loadingMessage="Loading your mentor profile..."
      loadingType="spinner"
    >
      {({ user, mentor }: any) => (
        <AlreadyMentorContent user={user} mentor={mentor} />
      )}
    </ProfileStateWrapper>
  );
}
