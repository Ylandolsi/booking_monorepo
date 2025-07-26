import { Button } from '@/components/ui';
import type { ExperienceType } from '@/features/profile';
import type { User } from '@/types/api';
import { formatDate } from '@/utils';
import { useState } from 'react';
import { MdOutlineWork } from 'react-icons/md';
import { ExperienceForm } from './experience-form';

export function Experience({ userData }: { userData: User }) {
  const [editingExperienceId, setEditingExperienceId] = useState<number | null>(
    null,
  );

  const [isAddingNew, setIsAddingNew] = useState<boolean>(false);

  const handleEditSuccess = () => {
    setEditingExperienceId(null);
  };

  const handleEditCancel = () => {
    setEditingExperienceId(null);
  };

  const handleAddSuccess = () => {
    setIsAddingNew(false);
  };

  const handleAddCancel = () => {
    setIsAddingNew(false);
  };

  const renderExperienceItem = (exp: ExperienceType) => {
    if (exp?.id == editingExperienceId) {
      return (
        <ExperienceForm
          onSuccess={handleEditSuccess}
          onCancel={handleEditCancel}
          experience={exp}
          isEditing={true}
        />
      );
    }
    return (
      <div
        key={exp.id}
        className="flex mb-2 p-2 border border-border rounded-xl justify-between gap-3 relative items-center hover:scale-105"
      >
        <div className="flex flex-shrink-0 w-10 h-10 bg-primary/10 rounded-xl items-center justify-center">
          <MdOutlineWork className="w-6 h-6 text-primary" />
        </div>
        <div className="flex-1 ">
          <div className="font-semibold text-sm text-foreground">
            {exp.title}
          </div>
          <div className="text-xs text-muted-foreground">
            {exp.company}
            <br></br>
            {formatDate(exp.startDate)} -{' '}
            {exp.endDate ? formatDate(exp.endDate) : 'Present'}
          </div>
          {exp.description && (
            <div className="text-xs text-muted-foreground mt-1 line-clamp-2">
              {exp.description}
            </div>
          )}
        </div>
        <Button
          variant="ghost"
          className="rounded-md hover:bg-border/20 "
          onClick={() => {
            setEditingExperienceId(exp.id!);
          }}
        >
          {' '}
          Edit{' '}
        </Button>
      </div>
    );
  };

  return (
    <div className="flex flex-col">
      {userData?.experiences.map((exp) => {
        return renderExperienceItem(exp);
      })}
      {isAddingNew ? (
        <ExperienceForm
          onSuccess={handleAddSuccess}
          onCancel={handleAddCancel}
          isEditing={false}
        />
      ) : (
        <Button
          variant={'outline'}
          className="border-border"
          onClick={() => setIsAddingNew(true)}
        >
          Add new experience
        </Button>
      )}
    </div>
  );
}
