import {
  Input,
  MultiSelect,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
  Textarea,
} from '@/components/ui';
import { Label } from '@/components/ui';
import { LANGUAGES_OPTIONS } from '../../constants';
import { UserDataWrapper } from '@/features/auth';
import { useState } from 'react';

type FieldOption = { value: string; label: string };

interface Field {
  key: string;
  label: string;
  type: 'file' | 'text' | 'select' | 'multiselect' | 'textarea';
  placeholder?: string;
  accept?: string;
  options?: FieldOption[];
  maxCount?: number;
  animation?: number;
}

const fields: Field[] = [
  {
    key: 'picture',
    label: 'Upload Picture',
    type: 'file',
    accept: 'image/*',
  },
  {
    key: 'firstName',
    label: 'First Name',
    type: 'text',
    placeholder: 'Enter your first name',
  },
  {
    key: 'lastName',
    label: 'Second Name',
    type: 'text',
    placeholder: 'Enter your second name',
  },
  {
    key: 'gender',
    label: 'Gender',
    type: 'select',
    options: [
      { value: 'Male', label: 'Male' },
      { value: 'Female', label: 'Female' },
    ],
    placeholder: 'Select a gender',
  },
  /*
  {
    key: 'country',
    label: 'Country',
    type: 'select',
    options: COUNTRIES,
    placeholder: 'Select a country',
  },
  */
  {
    key: 'languages',
    label: 'Languages',
    type: 'multiselect',
    options: LANGUAGES_OPTIONS,
    placeholder: 'Select up to 4 languages',
    maxCount: 4,
    animation: 2,
  },
  {
    key: 'bio',
    label: 'Bio',
    type: 'textarea',
    placeholder: 'Write a short bio',
  },
];

export function BasicInfoForm() {
  const [selectedGender, setSelectedGender] = useState<string | undefined>(
    undefined,
  );
  const [selectedLanguages, setSelectedLanguages] = useState<string[]>([]);

  return (
    <UserDataWrapper>
      {(userData) => (
        <div className="space-y-4">
          {fields.map((field) => {
            const defaultValue =
              field.key === 'languages'
                ? userData?.languages?.map((ld) => ld.name) || []
                : (userData?.[field.key as keyof typeof userData] ?? '');

            return (
              <div key={field.key} className="space-y-2">
                <Label className="block text-sm font-medium">
                  {field.label}
                </Label>

                {field.type === 'file' && (
                  <Input type="file" accept={field.accept} />
                )}

                {field.type === 'text' && (
                  <Input
                    type="text"
                    placeholder={field.placeholder}
                    defaultValue={defaultValue as string}
                  />
                )}

                {field.type === 'select' && (
                  <Select
                    onValueChange={(val) => {
                      if (field.key === 'gender') setSelectedGender(val);
                      // handle other select states if needed
                    }}
                    defaultValue={defaultValue as string}
                  >
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder={field.placeholder} />
                    </SelectTrigger>
                    <SelectContent>
                      {field.options?.map((option) => (
                        <SelectItem key={option.value} value={option.value}>
                          {option.value}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                )}

                {field.type === 'multiselect' && (
                  <MultiSelect
                    options={field.options!}
                    onValueChange={(val) => {
                      if (field.key === 'languages') setSelectedLanguages(val);
                    }}
                    defaultValue={defaultValue as string[]}
                    placeholder={field.placeholder}
                    animation={field.animation}
                    maxCount={field.maxCount}
                  />
                )}

                {field.type === 'textarea' && (
                  <Textarea
                    placeholder={field.placeholder}
                    defaultValue={defaultValue as string}
                  />
                )}
              </div>
            );
          })}
        </div>
      )}
    </UserDataWrapper>
  );
}
