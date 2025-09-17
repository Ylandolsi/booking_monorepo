import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage, Input, Button, Textarea } from '@/components/ui';
import { createStoreSchema } from '../lib';
import { useCreateStore } from '../hooks';
import type { CreateStoreInput } from '../types';

export const CreateStoreForm = () => {
  const createStoreMutation = useCreateStore();
  const form = useForm<CreateStoreInput>({
    resolver: zodResolver(createStoreSchema),
    defaultValues: {
      title: '',
      slug: '',
      description: '',
    },
  });

  const onSubmit = (values: CreateStoreInput) => {
    createStoreMutation.mutate(values);
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
        <FormField
          control={form.control}
          name="title"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Store Name</FormLabel>
              <FormControl>
                <Input placeholder="My Awesome Store" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="slug"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Store Slug</FormLabel>
              <FormControl>
                <Input placeholder="my-awesome-store" {...field} />
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
              <FormLabel>Description</FormLabel>
              <FormControl>
                <Textarea placeholder="A short description of your store." {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <Button type="submit" disabled={createStoreMutation.isPending}>
          {createStoreMutation.isPending ? 'Creating...' : 'Create Store'}
        </Button>
      </form>
    </Form>
  );
};
