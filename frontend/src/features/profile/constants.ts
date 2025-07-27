export const PROFILE_SECTIONS = [
  'Basic Info',
  'Career',
  'Social Links',
] as const;

export const COUNTRIES = [
  'United States',
  'Canada',
  'United Kingdom',
  'Germany',
] as const;

export const LANGUAGES_OPTIONS: {
  value: string;
  label: string;
}[] = [
  { value: 'English', label: 'English' },
  { value: 'French', label: 'French' },
  { value: 'Arabic', label: 'Arabic' },
] as const;
