import { Instagram, Twitter, Facebook, Youtube, Globe, Linkedin, Plus, Check } from 'lucide-react';
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '@/components/ui/accordion';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { Checkbox } from '@/components/ui/checkbox';
import type { UseFormReturn } from 'react-hook-form';
import type { PatchPostStoreRequest } from '@/api/stores';
import { FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Button, Input } from '@/components';
import { useState } from 'react';
import { FaTiktok } from 'react-icons/fa';

export const socialPlatforms = [
  { key: 'instagram', label: 'Instagram', icon: Instagram },
  { key: 'linkedin', label: 'LinkedIn', icon: Linkedin },
  { key: 'twitter', label: 'Twitter', icon: Twitter },
  { key: 'facebook', label: 'Facebook', icon: Facebook },
  { key: 'youtube', label: 'YouTube', icon: Youtube },
  { key: 'website', label: 'Website', icon: Globe },
  { key: 'tiktok', label: 'TikTok', icon: FaTiktok },
];

export const SocialLinksForm = ({ form }: { form: UseFormReturn<PatchPostStoreRequest> }) => {
  const [selectedPlatforms, setSelectedPlatforms] = useState<Set<string>>(
    new Set<string>([...(form.watch('socialLinks')?.map((link: any) => link.platform) || [])]),
  ); // default selected
  const [isPopoverOpen, setIsPopoverOpen] = useState(false);

  const selectedPlatformsArray = Array.from(selectedPlatforms);
  // we will show instagram and linkedin by default (first 2 in the socialPlatforms array)

  // availablePlatforms : the platforms that the user can add from the popover

  const availablePlatforms = socialPlatforms.filter((p) => !selectedPlatformsArray.includes(p.key));
  const handleAddPlatforms = () => {
    setIsPopoverOpen(false);
  };

  const handleCheckboxChange = (key: string, checked: boolean) => {
    if (checked) {
      setSelectedPlatforms((prev) => new Set(prev).add(key));
    } else {
      setSelectedPlatforms((prev) => {
        const newSet = new Set(prev);
        newSet.delete(key);
        return newSet;
      });
    }
  };

  return (
    <Accordion type="single" collapsible className="w-full">
      <AccordionItem value="social-links" className="border-0">
        <AccordionTrigger className="hover:bg-accent/30 rounded-lg px-3 py-2 transition-colors hover:no-underline">
          <div className="flex items-center gap-2 text-sm font-medium">
            <Globe className="h-4 w-4" />
            Social Links (Optional)
          </div>
        </AccordionTrigger>
        <AccordionContent className="space-y-4 pt-3">
          {/* Dynamically added platforms */}
          {selectedPlatformsArray.map((key) => {
            const platform = socialPlatforms.find((p) => p.key === key);
            if (!platform) return null;
            const { label, icon: Icon } = platform;
            return (
              <FormField
                key={key}
                control={form.control}
                name="socialLinks"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel className="text-foreground flex items-center gap-2 text-sm font-medium">
                      <Icon className="h-4 w-4" />
                      {label}
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder={`https://${key}.com/your-profile`}
                        className="border-border text-foreground h-11 rounded-lg"
                        value={field.value?.find((link: any) => link.platform === key)?.url || ''}
                        onChange={(e) => {
                          const currentLinks = field.value || [];
                          const existingIndex = currentLinks.findIndex((link: any) => link.platform === key);
                          if (existingIndex >= 0) {
                            currentLinks[existingIndex].url = e.target.value;
                          } else {
                            currentLinks.push({ platform: key, url: e.target.value });
                          }
                          field.onChange(currentLinks.filter((link: any) => link.url)); // filter out empty urls
                        }}
                      />
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
                  className="border-border/60 bg-accent/30 text-foreground hover:border-primary/50 hover:bg-accent/50 flex cursor-pointer items-center justify-center gap-2 rounded-lg border border-dashed px-4 py-3 text-sm font-medium transition-all duration-200"
                  onClick={() => {
                    setIsPopoverOpen(true);
                  }}
                >
                  <Plus className="h-4 w-4" />
                  Add Another Platform
                </div>
              </PopoverTrigger>
              <PopoverContent className="w-72 p-4" side="top" align="center">
                <div className="grid gap-4">
                  <div className="space-y-1">
                    <h4 className="text-sm font-semibold">Select Platforms</h4>
                    <p className="text-muted-foreground text-xs">Choose which platforms to add</p>
                  </div>
                  <div className="grid gap-3">
                    {availablePlatforms.map(({ key, label, icon: Icon }) => (
                      <div key={key} className="hover:bg-accent/50 flex items-center space-x-3 rounded-lg p-2 transition-colors">
                        <Checkbox
                          id={key}
                          checked={selectedPlatformsArray.includes(key)}
                          onCheckedChange={(checked) => {
                            handleCheckboxChange(key, checked as boolean);
                          }}
                        />
                        <label
                          htmlFor={key}
                          className="flex flex-1 cursor-pointer items-center gap-2 text-sm font-medium peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                        >
                          <Icon className="text-muted-foreground h-4 w-4" />
                          {label}
                        </label>
                      </div>
                    ))}
                    <Button
                      type="button"
                      size="sm"
                      onClick={handleAddPlatforms}
                      disabled={selectedPlatformsArray.length === 0}
                      className="mt-1 h-9 w-full rounded-lg"
                    >
                      <Check className="h-4 w-4" />
                      Add Selected ({selectedPlatformsArray.length})
                    </Button>
                  </div>
                </div>
              </PopoverContent>
            </Popover>
          )}
        </AccordionContent>
      </AccordionItem>
    </Accordion>
  );
};
