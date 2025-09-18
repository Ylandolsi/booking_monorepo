import { createFileRoute, useNavigate } from '@tanstack/react-router';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { IPhoneMockup } from 'react-device-mockup';
import { useCreateStore } from '@/features/store/hooks';
import { createStoreSchema } from '@/features/store/lib';
import type { CreateStoreInput } from '@/features/store/types';
import { Button } from '@/components/ui/button';
import { Card } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Label } from '@/components/ui/label';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { Checkbox } from '@/components/ui/checkbox';
import { Store, Upload, User, Link, CheckCircle, Calendar, Instagram, Twitter, Facebook, Youtube, Globe, Plus, Check } from 'lucide-react';
import { ROUTE_PATHS } from '@/config/routes';

export const Route = createFileRoute('/app/setup')({
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = useNavigate();
  const createStoreMutation = useCreateStore();
  const [previewImage, setPreviewImage] = useState<string>('');
  const [additionalPlatforms, setAdditionalPlatforms] = useState<string[]>([]);
  const [selectedPlatforms, setSelectedPlatforms] = useState<string[]>([]);
  const [isPopoverOpen, setIsPopoverOpen] = useState(false);

  const form = useForm<CreateStoreInput>({
    resolver: zodResolver(createStoreSchema),
    defaultValues: {
      title: '',
      slug: '',
      description: '',
      socialLinks: {
        instagram: '',
        twitter: '',
        facebook: '',
        youtube: '',
        website: '',
      },
    },
  });

  const watchedValues = form.watch();

  const generateSlug = (title: string) => {
    const slug = title
      .toLowerCase()
      .replace(/[^a-z0-9]/g, '-')
      .replace(/-+/g, '-')
      .replace(/^-|-$/g, '');
    form.setValue('slug', slug);
  };

  const handleImagePreview = (url: string) => {
    setPreviewImage(url);
  };

  const onSubmit = async (data: any) => {
    const socialLinksArray = Object.entries(data.socialLinks || {})
      .map(([platform, url]) => ({ platform, url: url as string }))
      .filter((link) => link.url);
    try {
      await createStoreMutation.mutateAsync({ ...data, socialLinks: socialLinksArray });
      navigate({ to: ROUTE_PATHS.APP.STORE });
    } catch (error) {
      console.error('Failed to create store:', error);
    }
  };

  const socialPlatforms = [
    { key: 'instagram', label: 'Instagram', icon: Instagram },
    { key: 'twitter', label: 'Twitter', icon: Twitter },
    { key: 'facebook', label: 'Facebook', icon: Facebook },
    { key: 'youtube', label: 'YouTube', icon: Youtube },
    { key: 'website', label: 'Website', icon: Globe },
  ];

  const availablePlatforms = socialPlatforms.filter((p) => !additionalPlatforms.includes(p.key) && !['instagram', 'twitter'].includes(p.key));

  const handleAddPlatforms = () => {
    setAdditionalPlatforms([...additionalPlatforms, ...selectedPlatforms]);
    setSelectedPlatforms([]);
    setIsPopoverOpen(false);
  };

  const handleCheckboxChange = (key: string, checked: boolean) => {
    if (checked) {
      setSelectedPlatforms([...selectedPlatforms, key]);
    } else {
      setSelectedPlatforms(selectedPlatforms.filter((p) => p !== key));
    }
  };

  return (
    <div className="flex flex-col md:flex-row w-full   gap-8 items-start justify-around">
      {/* Setup Form */}
      <div className="flex-1 max-w-lg">
        <div className="text-center mb-8">
          <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-primary to-scondary rounded-2xl mb-4">
            <Store className="w-8 h-8 text-primary-foreground" />
          </div>
          <h1 className="text-3xl font-bold bg-gradient-to-r from-primary to-scondary bg-clip-text text-transparent">Create Your Linki Store</h1>
          <p className="text-muted-foreground mt-2">Set up your personal mobile store in seconds</p>
        </div>

        <Card className="p-6 border-0 shadow-xl bg-card/80 backdrop-blur-sm animate-in fade-in duration-500">
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
              <FormField
                control={form.control}
                name="title"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="flex items-center gap-2 text-foreground">
                      <User className="w-4 h-4" />
                      Store Name *
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder="Your Amazing Store"
                        className="text-lg py-3  border-border text-foreground"
                        {...field}
                        onChange={(e) => {
                          field.onChange(e);
                          if (!watchedValues.slug) generateSlug(e.target.value);
                        }}
                      />
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
                    <FormLabel className="flex items-center gap-2 text-foreground">
                      <Link className="w-4 h-4" />
                      Unique Slug *
                    </FormLabel>
                    <div className="flex">
                      <div className=" text-center flex items-center justify-center px-2 bg-muted border border-r-0 rounded-l-md text-sm text-muted-foreground">
                        linki.store/
                      </div>
                      <FormControl>
                        <Input placeholder="your-store" className="rounded-l-none text-lg py-3  border-border text-foreground" {...field} />
                      </FormControl>
                    </div>
                    <p className="text-xs text-muted-foreground">This will be your store's URL: linki.store/{watchedValues.slug || 'your-store'}</p>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="text-foreground">Store Description</FormLabel>
                    <FormControl>
                      <Textarea placeholder="Tell your customers what you offer..." rows={3} className=" border-border text-foreground" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <div className="space-y-2">
                <Label className="flex items-center gap-2 text-foreground">
                  <Upload className="w-4 h-4" />
                  Profile Picture (Optional)
                </Label>
                <Input
                  type="url"
                  placeholder="https://example.com/your-photo.jpg"
                  className=" border-border text-foreground"
                  onChange={(e) => handleImagePreview(e.target.value)}
                />
                <p className="text-xs text-muted-foreground">Enter a URL to your profile picture for live preview</p>
              </div>

              {/* Collapsible Social Media Section */}
              <Accordion type="single" collapsible className="w-full">
                <AccordionItem value="social-links">
                  <AccordionTrigger className="text-foreground hover:text-primary">
                    <div className="flex items-center gap-2">
                      <Globe className="w-4 h-4" />
                      Add Social Links (Optional)
                    </div>
                  </AccordionTrigger>
                  <AccordionContent className="space-y-4">
                    {/* Default platforms */}
                    {socialPlatforms.slice(0, 2).map(({ key, label, icon: Icon }) => (
                      <FormField
                        key={key}
                        control={form.control}
                        name={`socialLinks.${key}` as any}
                        render={({ field }) => (
                          <FormItem>
                            <FormLabel className="flex items-center gap-2 text-foreground">
                              <Icon className="w-4 h-4" />
                              {label}
                            </FormLabel>
                            <FormControl>
                              <Input placeholder={`https://${key}.com/your-profile`} className=" border-border text-foreground" {...field} />
                            </FormControl>
                            <FormMessage />
                          </FormItem>
                        )}
                      />
                    ))}

                    {/* Dynamically added platforms */}
                    {additionalPlatforms.map((key) => {
                      const platform = socialPlatforms.find((p) => p.key === key);
                      if (!platform) return null;
                      const { label, icon: Icon } = platform;
                      return (
                        <FormField
                          key={key}
                          control={form.control}
                          name={`socialLinks.${key}` as any}
                          render={({ field }) => (
                            <FormItem>
                              <FormLabel className="flex items-center gap-2 text-foreground">
                                <Icon className="w-4 h-4" />
                                {label}
                              </FormLabel>
                              <FormControl>
                                <Input placeholder={`https://${key}.com/your-profile`} className=" border-border text-foreground" {...field} />
                              </FormControl>
                              <FormMessage />
                            </FormItem>
                          )}
                        />
                      );
                    })}
                    {/* Add more button with popover */}
                    {availablePlatforms.length > 0 && (
                      <Popover open={isPopoverOpen} onOpenChange={setIsPopoverOpen}>
                        <PopoverTrigger asChild>
                          <div
                            className="  w-full bg-gradient-to-br from-primary to-primary/20 text-background p-3 rounded-md flex items-center gap-2 border-border hover:bg-primary transition-all duration-200"
                            onClick={() => {
                              setIsPopoverOpen(true);
                            }}
                          >
                            <Plus className="w-4 h-4" />
                            Add Another Platform
                          </div>
                        </PopoverTrigger>
                        <PopoverContent className="w-64 p-4" side="top" align="center">
                          <div className="grid gap-4">
                            <div className="space-y-2">
                              <h4 className="font-medium leading-none">Select Platforms</h4>
                              <p className="text-sm text-muted-foreground">Choose which platforms to add</p>
                            </div>
                            <div className="grid gap-2">
                              {availablePlatforms.map(({ key, label, icon: Icon }) => (
                                <div key={key} className="flex items-center space-x-2">
                                  <Checkbox
                                    id={key}
                                    checked={selectedPlatforms.includes(key)}
                                    onCheckedChange={(checked) => {
                                      handleCheckboxChange(key, checked as boolean);
                                    }}
                                  />
                                  <label
                                    htmlFor={key}
                                    className="flex items-center gap-2 text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70 cursor-pointer"
                                  >
                                    <Icon className="w-4 h-4" />
                                    {label}
                                  </label>
                                </div>
                              ))}
                              <Button
                                type="button"
                                size="sm"
                                onClick={handleAddPlatforms}
                                disabled={selectedPlatforms.length === 0}
                                className="w-full mt-2"
                              >
                                <Check className="w-4 h-4 mr-2" />
                                Add Selected ({selectedPlatforms.length})
                              </Button>
                            </div>
                          </div>
                        </PopoverContent>
                      </Popover>
                    )}
                  </AccordionContent>
                </AccordionItem>
              </Accordion>

              <Button
                type="submit"
                size="lg"
                className="w-full bg-gradient-to-r from-primary to-scondary hover:from-primary hover:to-accent text-primary-foreground transition-all duration-300"
                disabled={createStoreMutation.isPending}
              >
                {createStoreMutation.isPending ? (
                  'Creating Store...'
                ) : (
                  <>
                    <CheckCircle className="w-4 h-4 mr-2" />
                    Create My Store
                  </>
                )}
              </Button>
            </form>
          </Form>
        </Card>
      </div>

      {/* Live Preview - keeping the same */}
      <div className="flex-1 max-w-sm animate-in slide-in-from-right duration-700">
        <div className="text-center mb-4">
          <h3 className="text-lg font-semibold text-foreground">Live Preview</h3>
          <p className="text-sm text-muted-foreground">See how your store will look</p>
        </div>

        <div className="flex justify-center">
          <IPhoneMockup screenWidth={320}>
            <div className="p-6 text-center bg-background min-h-full">
              <div className="w-20 h-20 bg-gradient-to-r from-primary to-scondary rounded-full mx-auto mb-4 flex items-center justify-center">
                {previewImage ? (
                  <img src={previewImage} alt="Profile" className="w-full h-full rounded-full object-cover" onError={() => setPreviewImage('')} />
                ) : (
                  <User className="w-8 h-8 text-primary-foreground" />
                )}
              </div>

              <h2 className="text-xl font-bold text-foreground mb-2">{watchedValues.title || 'Your Store Name'}</h2>

              <p className="text-muted-foreground text-sm mb-4 leading-relaxed">
                {watchedValues.description || 'Your store description will appear here...'}
              </p>

              {watchedValues.socialLinks && Object.values(watchedValues.socialLinks).some((link) => link) && (
                <div className="flex justify-center gap-4 mb-6">
                  {socialPlatforms.map(
                    ({ key, icon: Icon }) =>
                      watchedValues.socialLinks?.[key as keyof typeof watchedValues.socialLinks] && (
                        <a
                          key={key}
                          href={watchedValues.socialLinks[key as keyof typeof watchedValues.socialLinks]}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="text-primary hover:text-accent transition-colors"
                        >
                          <Icon className="w-5 h-5" />
                        </a>
                      ),
                  )}
                </div>
              )}

              {/* <div className="space-y-3">
                <div className="bg-muted rounded-lg p-4 text-left border border-border">
                  <div className="flex items-center gap-2 mb-1">
                    <Calendar className="w-3 h-3 text-primary" />
                    <span className="text-xs text-muted-foreground">1:1 Coaching Call</span>
                  </div>
                  <div className="font-semibold text-foreground">Book a Call</div>
                  <div className="text-primary font-semibold">$99</div>
                </div>
                <div className="bg-muted rounded-lg p-4 text-left border border-border">
                  <div className="text-xs text-muted-foreground mb-1">Sample Product</div>
                  <div className="font-semibold text-foreground">Digital Guide</div>
                  <div className="text-primary font-semibold">$29</div>
                </div>
              </div> */}
            </div>
          </IPhoneMockup>
        </div>
      </div>
    </div>
  );
}
