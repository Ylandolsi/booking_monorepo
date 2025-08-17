import { Button, Label } from '@/components/ui';
import type { User } from '@/types/api';
import { formatDate } from '@/utils';
import { useState } from 'react';
import { FaBookReader } from 'react-icons/fa';
import { EducationForm } from './education-form';
import type { EducationType } from '@/features/app/profile';

export function Education({ userData }: { userData: User }) {
  const [editingEducationId, setEditingEducationId] = useState<number | null>(
    null,
  );
  const [isAddingNew, setIsAddingNew] = useState<boolean>(false);

  const handleEditSuccess = () => {
    setEditingEducationId(null);
  };

  const handleEditCancel = () => {
    setEditingEducationId(null);
  };

  const handleAddSuccess = () => {
    setIsAddingNew(false);
  };

  const handleAddCancel = () => {
    setIsAddingNew(false);
  };

  const renderEducationItem = (education: EducationType) => {
    const isCurrentlyEditing = editingEducationId === education.id;

    if (isCurrentlyEditing) {
      return (
        <EducationForm
          key={`edit-${education.id}`}
          education={education}
          onSuccess={handleEditSuccess}
          onCancel={handleEditCancel}
          isEditing={true}
        />
      );
    }

    return (
      <div
        key={education.id}
        className="border border-border flex justify-between items-center p-2 rounded-xl gap-3 hover:scale-105 transition-transform"
      >
        <div className="w-10 h-10 flex items-center justify-center rounded-xl bg-blue-50">
          <FaBookReader className="w-6 h-6 text-blue-600" />
        </div>
        <div className="flex-1">
          <div className="font-semibold text-sm">{education.field}</div>
          <div className="text-xs text-muted-foreground">
            {education.university}
            <br />
            {formatDate(education.startDate)} -{' '}
            {education.endDate ? formatDate(education.endDate) : 'Present'}
          </div>
          {education.description && (
            <div className="text-xs text-muted-foreground mt-1 line-clamp-2">
              {education.description}
            </div>
          )}
        </div>
        <Button
          variant="ghost"
          className="hover:bg-border/20"
          onClick={() => setEditingEducationId(education.id || null)}
        >
          Edit
        </Button>
      </div>
    );
  };

  return (
    <div className="space-y-2">
      <Label className="block text-sm font-medium">Education</Label>
      <div className="flex flex-col gap-2">
        {userData?.educations?.map((education) =>
          renderEducationItem(education),
        )}

        {isAddingNew ? (
          <EducationForm
            key="add-new"
            onSuccess={handleAddSuccess}
            onCancel={handleAddCancel}
            isEditing={false}
          />
        ) : (
          <Button
            variant="outline"
            className="border-border mt-2 mb-2"
            onClick={() => setIsAddingNew(true)}
          >
            Add New Education
          </Button>
        )}
      </div>
    </div>
  );
}
