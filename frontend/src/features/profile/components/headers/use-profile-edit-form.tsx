import { useState } from 'react';

export function useProfileEditForm() {
  const [sectionSelected, setSectionSelected] = useState<string>('Basic Info');
  const [selectedGender, setSelectedGender] = useState<string | undefined>();
  const [selectedCountry, setSelectedCountry] = useState<string | undefined>();
  const [selectedExpertise, setSelectedExpertise] = useState<string[]>([]);
  const [selectedLanguages, setSelectedLanguages] = useState<string[]>([]);

  const handleSubmit = (section: string) => {
    // Handle form submission logic here
    console.log(`Submitting ${section}`, {
      selectedGender,
      selectedCountry,
      selectedExpertise,
      selectedLanguages,
    });
  };

  return {
    sectionSelected,
    setSectionSelected,
    selectedGender,
    setSelectedGender,
    selectedCountry,
    setSelectedCountry,
    selectedExpertise,
    setSelectedExpertise,
    selectedLanguages,
    setSelectedLanguages,
    handleSubmit,
  };
}
