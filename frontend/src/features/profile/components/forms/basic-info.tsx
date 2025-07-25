import {
  Input,
  MultiSelect,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
  Spinner,
  Textarea,
} from '@/components/ui';
import { Label } from '@radix-ui/react-label';
import { COUNTRIES, LANGUAGES_OPTIONS } from '../../constants';
import { useUser } from '@/features/auth';
import { MainErrorFallback } from '@/components/errors/main';

export function BasicInfoForm({
  selectedGender,
  setSelectedGender,
  selectedCountry,
  setSelectedCountry,
  selectedLanguages,
  setSelectedLanguages,
}: {
  selectedGender: string | undefined;
  setSelectedGender: (value: string | undefined) => void;
  selectedCountry: string | undefined;
  setSelectedCountry: (value: string | undefined) => void;
  selectedLanguages: string[];
  setSelectedLanguages: (value: string[]) => void;
}) {
  const { data: userData, error, isLoading } = useUser();
  if (isLoading) return <Spinner />;
  if (error) return <MainErrorFallback />;
  return (
    <div className="space-y-4">
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Upload Picture</Label>
        <Input type="file" accept="image/*" />
      </div>
      <div className="space-y-2">
        <Label className="block text-sm font-medium">First Name</Label>
        <Input
          type="text"
          placeholder="Enter your first name"
          value={userData?.firstName}
        />
      </div>
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Second Name</Label>
        <Input
          type="text"
          placeholder="Enter your second name"
          defaultValue={userData?.lastName}
        />
      </div>
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Gender</Label>
        <Select
          onValueChange={setSelectedGender}
          defaultValue={userData?.gender}
        >
          <SelectTrigger className="w-full">
            <SelectValue placeholder="Select a gender" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="Male">Male</SelectItem>
            <SelectItem value="Female">Female</SelectItem>
          </SelectContent>
        </Select>
      </div>
      {/* <div className="space-y-2">
        <Label className="block text-sm font-medium">Country</Label>
        <Select onValueChange={setSelectedCountry}>
          <SelectTrigger className="w-full">
            <SelectValue placeholder="Select a country" />
          </SelectTrigger>
          <SelectContent>
            {COUNTRIES.map((country) => (
              <SelectItem key={country} value={country}>
                {country}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div> */}
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Languages</Label>
        <MultiSelect
          options={[...LANGUAGES_OPTIONS]}
          onValueChange={setSelectedLanguages}
          defaultValue={userData?.languages.map((ld) => ld.name)}
          placeholder="Select up to 4 languages"
          variant="inverted"
          animation={2}
          maxCount={4}
        />
      </div>
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Bio</Label>
        <Textarea
          placeholder="Write a short bio"
          defaultValue={userData?.bio}
        />
      </div>
    </div>
  );
}
