import { zodResolver } from '@hookform/resolvers/zod';
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
import type { ExperienceType } from '@/features/profile';
import {
  useAddExperience,
  useDeleteExperience,
  useUpdateExperience,
} from '@/features/profile';
import {
  experienceSchema,
  type ExperienceInput,
} from '@/features/profile/schemas/career';
import { useMemo } from 'react';
import { useForm } from 'react-hook-form';

interface ExperienceFormProps {
  experience?: ExperienceType;
  onSuccess: () => void;
  onCancel: () => void;
  isEditing?: boolean;
}
export function ExperienceForm({
  experience,
  onSuccess,
  onCancel,
  isEditing,
}: ExperienceFormProps) {
  const addExperienceMutation = useAddExperience();
  const deleteExperienceMutation = useDeleteExperience();
  const updateExperienceMutation = useUpdateExperience();

  const form = useForm<ExperienceInput>({
    resolver: zodResolver(experienceSchema),
    defaultValues: {
      title: experience?.title || '',
      company: experience?.company || '',
      description: experience?.description || '',

      startDate: experience?.startDate
        ? new Date(experience.startDate).getFullYear().toString()
        : '',
      endDate: experience?.endDate
        ? new Date(experience.endDate).getFullYear().toString()
        : '',

      toPresent: experience?.toPresent || false,
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

  const onSubmit = async (data: ExperienceInput) => {
    const experienceData: ExperienceType = {
      title: data.title,
      company: data.company,
      description: data.description || '',
      startDate: new Date(`${data.startDate}-01-01`),
      endDate:
        data.toPresent || !data.endDate
          ? null
          : new Date(`${data.endDate}-01-01`),
      toPresent: data.toPresent,
    };
    try {
      if (isEditing && experience?.id) {
        await updateExperienceMutation.mutateAsync({
          id: experience.id,
          data: experienceData,
        });
      } else {
        await addExperienceMutation.mutateAsync({
          experience: experienceData,
        });
      }
      onSuccess();
    } catch (error) {
      console.error('Failed to save experience:', error);
    }
  };
  const handleDelete = async () => {
    if (!experience?.id) return;

    try {
      await deleteExperienceMutation.mutateAsync({ id: experience.id });
      onSuccess();
    } catch (error) {
      console.error('Failed to delete experience:', error);
    }
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <div className="flex flex-col p-4 border border-border rounded-xl gap-5 w-full bg-gradient-to-br from-accent/50 to-accent/10">
          <FormField
            control={form.control}
            name="title"
            render={({ field }) => (
              <FormItem>
                <FormLabel className="text-sm font-medium">
                  Title / Position
                </FormLabel>
                <FormControl>
                  <Input
                    {...field}
                    className="text-foreground"
                    placeholder="Eg: Senior Software Enginner"
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="company"
            render={({ field }) => (
              <FormItem>
                <FormLabel className="text-sm font-medium">
                  Company Name
                </FormLabel>
                <FormControl>
                  <Input
                    {...field}
                    className="text-foreground"
                    placeholder="Eg: Google"
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
                    placeholder="Describe your experience..."
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
                addExperienceMutation.isPending ||
                updateExperienceMutation.isPending
              }
            >
              {isEditing ? 'Update Experience' : 'Save Experience'}
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
                disabled={deleteExperienceMutation.isPending}
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
