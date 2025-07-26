import {
  Button,
  Input,
  Label,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
  Textarea,
  Checkbox,
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui';

import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { useMemo } from 'react';
import {
  educationSchema,
  type EducationInput,
} from '@/features/profile/schemas/career';
import {
  useAddEducation,
  useDeleteEducation,
  useUpdateEducation,
  type EducationType,
} from '@/features/profile';

interface EducationFormProps {
  education?: EducationType;
  onSuccess: () => void;
  onCancel: () => void;
  isEditing?: boolean;
}

export function EducationForm({
  education,
  onSuccess,
  onCancel,
  isEditing = false,
}: EducationFormProps) {
  const addEducationMutation = useAddEducation();
  const updateEducationMutation = useUpdateEducation();
  const deleteEducationMutation = useDeleteEducation();

  const form = useForm<EducationInput>({
    resolver: zodResolver(educationSchema),
    defaultValues: {
      university: education?.university || '',
      field: education?.field || '',
      description: education?.description || '',
      startDate: education?.startDate
        ? new Date(education.startDate).getFullYear().toString()
        : '',
      endDate: education?.endDate
        ? new Date(education.endDate).getFullYear().toString()
        : '',
      toPresent: education?.toPresent || false,
    },
  });

  const generateYearOptions = useMemo(() => {
    const currentYear = new Date().getFullYear();
    const years = [];
    for (let year = currentYear; year >= 1950; year--) {
      years.push(year.toString());
    }
    return years;
  }, []);

  const watchToPresent = form.watch('toPresent');

  const onSubmit = async (data: EducationInput) => {
    const educationData: EducationType = {
      university: data.university,
      field: data.field,
      description: data.description || '',
      startDate: new Date(`${data.startDate}-01-01`),
      endDate:
        data.toPresent || !data.endDate
          ? null
          : new Date(`${data.endDate}-01-01`),
      toPresent: data.toPresent,
    };

    try {
      if (isEditing && education?.id) {
        await updateEducationMutation.mutateAsync({
          id: education.id,
          data: educationData,
        });
      } else {
        await addEducationMutation.mutateAsync({
          education: educationData,
        });
      }
      onSuccess();
    } catch (error) {
      console.error('Failed to save education:', error);
    }
  };

  const handleDelete = async () => {
    if (!education?.id) return;

    try {
      await deleteEducationMutation.mutateAsync({ id: education.id });
      onSuccess();
    } catch (error) {
      console.error('Failed to delete education:', error);
    }
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <div className="flex flex-col p-4 border border-border rounded-xl gap-5 w-full bg-gradient-to-br from-accent/50 to-accent/10">
          <FormField
            control={form.control}
            name="university"
            render={({ field }) => (
              <FormItem>
                <FormLabel className="text-sm font-medium">
                  University / College / School
                </FormLabel>
                <FormControl>
                  <Input
                    {...field}
                    className="text-foreground"
                    placeholder="Eg: University of Tunis"
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="field"
            render={({ field }) => (
              <FormItem>
                <FormLabel className="text-sm font-medium">
                  Degree / Field of Study
                </FormLabel>
                <FormControl>
                  <Input
                    {...field}
                    className="text-foreground"
                    placeholder="Eg: Bachelor in Computer Science"
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="description"
            render={({ field }) => (
              <FormItem>
                <FormLabel className="text-sm font-medium">
                  Description
                </FormLabel>
                <FormControl>
                  <Textarea
                    {...field}
                    placeholder="Describe your education experience..."
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <div className="space-y-4">
            <Label className="text-sm font-medium">Timeline</Label>
            <div className="flex items-start gap-4">
              <FormField
                control={form.control}
                name="startDate"
                render={({ field }) => (
                  <FormItem className="flex-1">
                    <FormLabel className="text-xs text-muted-foreground">
                      Start Year
                    </FormLabel>
                    <Select onValueChange={field.onChange} value={field.value}>
                      <FormControl>
                        <SelectTrigger className="w-full">
                          <SelectValue placeholder="Start" />
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent className="h-[200px]">
                        {generateYearOptions.map((year) => (
                          <SelectItem key={year} value={year}>
                            {year}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                    <FormMessage />
                  </FormItem>
                )}
              />

              {!watchToPresent && (
                <FormField
                  control={form.control}
                  name="endDate"
                  render={({ field }) => (
                    <FormItem className="flex-1">
                      <FormLabel className="text-xs text-muted-foreground">
                        End Year
                      </FormLabel>
                      <Select
                        onValueChange={field.onChange}
                        value={field.value || ''}
                      >
                        <FormControl>
                          <SelectTrigger className="w-full">
                            <SelectValue placeholder="End" />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent className="h-[200px]">
                          {generateYearOptions.map((year) => (
                            <SelectItem key={year} value={year}>
                              {year}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              )}
            </div>

            <FormField
              control={form.control}
              name="toPresent"
              render={({ field }) => (
                <FormItem className="flex flex-row items-start space-x-3 space-y-0">
                  <FormControl>
                    <Checkbox
                      checked={field.value}
                      onCheckedChange={(checked) => {
                        field.onChange(checked);
                        if (checked) {
                          form.setValue('endDate', '');
                        }
                      }}
                    />
                  </FormControl>
                  <div className="space-y-1 leading-none">
                    <FormLabel className="text-sm">
                      I am currently studying here
                    </FormLabel>
                  </div>
                </FormItem>
              )}
            />
          </div>

          <div className="gap-2 flex">
            <Button
              type="submit"
              className="flex-1"
              disabled={
                addEducationMutation.isPending ||
                updateEducationMutation.isPending
              }
            >
              {isEditing ? 'Update Education' : 'Save Education'}
            </Button>
            <Button
              type="button"
              className="flex-1"
              variant="outline"
              onClick={onCancel}
            >
              Cancel
            </Button>
            {isEditing && (
              <Button
                type="button"
                variant="destructive"
                className="bg-gradient-to-r from-red-500 to-red-700 text-background"
                onClick={handleDelete}
                disabled={deleteEducationMutation.isPending}
              >
                Delete
              </Button>
            )}
          </div>
        </div>
      </form>
    </Form>
  );
}
