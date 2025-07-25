import { DrawerDialog } from '@/components/ui';
import { Button } from '@/components/ui';
import { useProfileEditForm } from '../headers/use-profile-edit-form';
import { BasicInfoForm, ExperienceForm, SocialLinksForm } from '../forms';
import { PROFILE_SECTIONS } from '../../constants';

export function ProfileEditDialog() {
  const {
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
  } = useProfileEditForm();

  const renderSectionContent = () => {
    switch (sectionSelected) {
      case 'Basic Info':
        return (
          <BasicInfoForm
            selectedGender={selectedGender}
            setSelectedGender={setSelectedGender}
            selectedCountry={selectedCountry}
            setSelectedCountry={setSelectedCountry}
            selectedLanguages={selectedLanguages}
            setSelectedLanguages={setSelectedLanguages}
          />
        );
      case 'Experience':
        return (
          <ExperienceForm
            selectedExpertise={selectedExpertise}
            setSelectedExpertise={setSelectedExpertise}
          />
        );
      case 'Social Links':
        return <SocialLinksForm />;
      default:
        return null;
    }
  };

  return (
    <DrawerDialog
      trigger={
        <Button size="lg" className="rounded-xl w-full">
          Edit Profile
        </Button>
      }
      title="Edit Profile"
      description="Make changes to your profile here."
    >
      <div className="flex space-x-4 mb-5">
        {PROFILE_SECTIONS.map((section) => (
          <button
            key={section}
            className={`py-2 px-4 rounded-lg text-sm font-medium ${
              sectionSelected === section
                ? 'bg-primary text-white'
                : 'bg-muted text-foreground'
            }`}
            onClick={() => setSectionSelected(section)}
          >
            {section}
          </button>
        ))}
      </div>
      <div>{renderSectionContent()}</div>
    </DrawerDialog>
  );
}
