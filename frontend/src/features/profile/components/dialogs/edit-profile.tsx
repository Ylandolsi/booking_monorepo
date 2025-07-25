import { Button, DrawerDialog } from '@/components/ui';
import { BasicInfoForm, Career, SocialLinksForm } from '../edit';
import { PROFILE_SECTIONS } from '../../constants';
import { useEffect, useState } from 'react';
import { useProfileEditStore } from '../../stores/profile-edit-store';

export function GlobalProfileEditDialog() {
  const { isOpen, setIsOpen, defaultSection } = useProfileEditStore();

  const [sectionSelected, setSectionSelected] = useState(defaultSection);

  // Reset section when dialog opens or defaultSection changes
  useEffect(() => {
    if (isOpen) {
      setSectionSelected(defaultSection);
    }
  }, [isOpen, defaultSection]);

  const handleSubmit = (section: string) => {
    console.log(`Submitting ${section}`, {});
  };

  const renderSectionContent = () => {
    switch (sectionSelected) {
      case 'Basic Info':
        return <BasicInfoForm />;
      case 'Career':
        return <Career />;
      case 'Social Links':
        return <SocialLinksForm />;
      default:
        return null;
    }
  };

  return (
    <DrawerDialog
      open={isOpen}
      onOpenChange={setIsOpen}
      trigger={null}
      title="Edit Profile"
      description="Make changes to your profile here."
    >
      <div className="flex space-x-4 mb-5">
        {PROFILE_SECTIONS.map((section) => (
          <Button
            key={section}
            className={`py-2 px-4 rounded-lg text-sm font-medium ${
              sectionSelected === section
                ? 'bg-primary text-white'
                : 'bg-muted text-foreground hover:bg-border '
            }`}
            onClick={() => setSectionSelected(section)}
          >
            {section}
          </Button>
        ))}
      </div>
      <div>{renderSectionContent()}</div>
    </DrawerDialog>
  );
}
