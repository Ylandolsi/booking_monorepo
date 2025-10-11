import { useForm } from 'react-hook-form';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion';
import { User, CheckCircle, Store as StoreIcon } from 'lucide-react';
import routes from '@/config/routes';
import 'react-image-crop/dist/ReactCrop.css';
import { useUpdateStore, type PatchPostStoreRequest } from '@/api/stores';
import { SocialLinksForm, type StoreFormData } from '@/pages/store';
import { UploadImage } from '@/components';
import { useAppNavigation } from '@/hooks';
import { logger } from '@/lib';

export const StoreSection = ({ form, handleCloseDialog }: { form: ReturnType<typeof useForm<StoreFormData>>; handleCloseDialog: () => void }) => {
  const updateStoreMutation = useUpdateStore();
  const navigate = useAppNavigation();
  const onSubmit = async (data: PatchPostStoreRequest) => {
    try {
      logger.info('Submitting store data:', data);
      // todo : handle this api
      await updateStoreMutation.mutateAsync(data);

      // after upadte clean the cropped image
      handleCloseDialog();

      navigate.goTo({ to: routes.to.store.index() + '/' });
    } catch (error) {
      logger.error('Failed to update store:', error);
    }
  };

  return (
    <div className="bg-card/50 border-border/50 rounded-xl border shadow-sm backdrop-blur-sm">
      <Accordion type="single" collapsible className="w-full">
        <AccordionItem value="store-details" className="border-0">
          <AccordionTrigger className="hover:bg-accent/50 rounded-t-xl px-6 py-5 transition-colors hover:no-underline">
            <div className="flex items-center gap-3">
              <div className="bg-primary/10 text-primary rounded-lg p-2">
                <StoreIcon className="h-5 w-5" />
              </div>
              <h2 className="text-foreground text-xl font-semibold">Store Details</h2>
            </div>
          </AccordionTrigger>
          <AccordionContent className="px-6 pt-2 pb-6">
            <div className="space-y-5">
              <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-5">
                  <FormField
                    control={form.control}
                    name="title"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel className="text-foreground flex items-center gap-2 text-sm font-medium">
                          <User className="h-4 w-4" />
                          Store Name *
                        </FormLabel>
                        <FormControl>
                          <Input
                            placeholder="Your Amazing Store"
                            className="border-border text-foreground h-11 rounded-lg"
                            {...field}
                            onChange={(e) => {
                              field.onChange(e);
                            }}
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
                        <FormLabel className="text-foreground text-sm font-medium">Store Description</FormLabel>
                        <FormControl>
                          <Textarea
                            placeholder="Tell your customers what you offer..."
                            rows={3}
                            className="border-border text-foreground resize-none rounded-lg"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                  <UploadImage description="Profile Picture (Optional)" />

                  <SocialLinksForm form={form} />

                  <Button
                    type="submit"
                    size="lg"
                    className="h-11 w-full rounded-lg font-medium transition-all duration-200"
                    disabled={updateStoreMutation.isPending}
                  >
                    {updateStoreMutation.isPending ? (
                      'Updating Store...'
                    ) : (
                      <>
                        <CheckCircle className="h-4 w-4" />
                        Update Store Details
                      </>
                    )}
                  </Button>
                </form>
              </Form>
            </div>{' '}
          </AccordionContent>
        </AccordionItem>
      </Accordion>
    </div>
  );
};
