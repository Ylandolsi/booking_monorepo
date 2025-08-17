import { create } from 'zustand';
type Section = 'Basic Info' | 'Career' | 'Social Links';

type ProfileEditState = {
  isOpen: boolean;
  defaultSection: Section;
  selectedGender?: string;
  selectedCountry?: string;
  selectedExpertise: number[];
  selectedLanguages: number[];

  // Actions
  openDialog: () => void;
  closeDialog: () => void;
  setDefaultSection: (section: Section) => void;
  setIsOpen: (open: boolean) => void;

  // Form actions
  setSelectedGender: (gender?: string) => void;
  setSelectedCountry: (country?: string) => void;
  setSelectedExpertise: (expertise: number[]) => void;
  setSelectedLanguages: (languages: number[]) => void;
  resetForm: () => void;
};

export const useProfileEditStore = create<ProfileEditState>((set) => ({
  isOpen: false,
  defaultSection: 'Basic Info',
  selectedGender: undefined,
  selectedCountry: undefined,
  selectedExpertise: [],
  selectedLanguages: [],

  // Dialog actions
  openDialog: () => set({ isOpen: true }),
  closeDialog: () => set({ isOpen: false }),
  setDefaultSection: (section: Section) => set({ defaultSection: section }),
  setIsOpen: (open: boolean) => set({ isOpen: open }),

  // Form actions
  setSelectedGender: (gender?: string) => set({ selectedGender: gender }),
  setSelectedCountry: (country?: string) => set({ selectedCountry: country }),
  setSelectedExpertise: (expertise: number[]) => {
    console.log('expertise:', expertise);
    set({ selectedExpertise: expertise });
  },

  setSelectedLanguages: (languages: number[]) =>
    set({ selectedLanguages: languages }),

  // Reset form data
  resetForm: () =>
    set({
      selectedGender: undefined,
      selectedCountry: undefined,
      selectedExpertise: [],
      selectedLanguages: [],
    }),
}));
