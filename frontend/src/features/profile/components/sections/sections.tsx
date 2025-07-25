import { useState } from 'react';
import { Overview } from './overview';

type sectonsType = 'Overview' | 'Mentors' | 'Calendar';

export function Sections() {
  let sections = ['Overview', 'Mentors', 'Calendar'];
  const [sectionSelected, setSectionSelected] =
    useState<sectonsType>('Overview');

  const handleSectionClick = (section: sectonsType) => {
    setSectionSelected(section);
  };

  const renderSectionContent = () => {
    switch (sectionSelected) {
      case 'Overview':
        return <Overview />;
      case 'Mentors':
        return (
          <div className="p-8 text-center text-muted-foreground">
            My Mentors Content Coming Soon
          </div>
        );
      case 'Calendar':
        return (
          <div className="p-8 text-center text-muted-foreground">
            My Calendar Coming Soon
          </div>
        );

      default:
        return (
          <div className="p-8 text-center text-muted-foreground">
            Overview Content
          </div>
        );
    }
  };

  return (
    <div className="w-full">
      {/* Section Tabs */}
      <div className="border-b border-border/50 bg-muted/20">
        <div className="flex justify-start sm:px-8">
          {sections.map((item, index) => (
            <button
              key={index}
              className={`relative py-4 px-6 font-medium transition-all duration-200 ${
                sectionSelected === item
                  ? 'text-primary border-b-2 border-primary bg-background/50'
                  : 'text-muted-foreground hover:text-foreground hover:bg-background/30'
              }`}
              onClick={() => handleSectionClick(item as sectonsType)}
            >
              {item}
            </button>
          ))}
        </div>
      </div>

      {/* Section Content */}
      <div className="bg-background">{renderSectionContent()}</div>
    </div>
  );
}

// import { z } from 'zod';
// import {
//   Button,
//   Input,
//   Label,
//   Select,
//   SelectContent,
//   SelectItem,
//   SelectTrigger,
//   SelectValue,
//   Textarea,
//   Checkbox,
// } from '@/components/ui';
// // ...other imports...

// // 1. Define a Zod schema for your form
// const EducationSchema = z.object({
//   university: z.string().min(1, 'University is required'),
//   field: z.string().min(1, 'Field of study is required'),
//   description: z.string().optional(),
//   startDate: z.string().regex(/^\d{4}$/, 'Start year must be four digits'),
//   endDate: z
//     .string()
//     .optional()
//     .regex(/^\d{4}$/, 'End year must be four digits'),
//   toPresent: z.boolean(),
// });

// type EducationInput = z.infer<typeof EducationSchema>;

// // ...existing code...

// export function Education({ userData }: { userData: User }) {
//   // ...state hooks...
//   const [errors, setErrors] = useState<
//     Partial<Record<keyof EducationInput, string>>
//   >({});

//   const handleSave = async () => {
//     // 2. Run safeParse against your state
//     const result = EducationSchema.safeParse(formData);
//     if (!result.success) {
//       // 3. Extract Zod errors and shove into local state
//       const fieldErrors = result.error.formErrors.fieldErrors;
//       setErrors(
//         Object.fromEntries(
//           Object.entries(fieldErrors).map(([k, v]) => [k, v?.[0]]),
//         ) as any,
//       );
//       return;
//     }

//     // 4. If valid, clear errors and proceed
//     setErrors({});
//     const validData = result.data;
//     const educationData: EducationType = {
//       university: validData.university,
//       field: validData.field,
//       description: validData.description,
//       startDate: new Date(`${validData.startDate}-01-01`),
//       endDate: validData.toPresent
//         ? null
//         : new Date(`${validData.endDate}-01-01`),
//       toPresent: validData.toPresent,
//     };

//     try {
//       if (educationToEdit) {
//         await updateEducationMutation.mutateAsync({
//           id: educationToEdit,
//           data: educationData,
//         });
//       } else {
//         await addEducationMutation.mutateAsync({ education: educationData });
//       }
//       handleCancel();
//     } catch (err) {
//       console.error('Failed to save education:', err);
//     }
//   };

//   const renderEducationForm = () => (
//     <div className="flex flex-col p-4 border rounded-xl gap-5">
//       {/* University */}
//       <div>
//         <Label>University</Label>
//         <Input
//           value={formData.university}
//           onChange={(e) => handleInputChange('university', e.target.value)}
//         />
//         {errors.university && (
//           <p className="text-red-500 text-sm">{errors.university}</p>
//         )}
//       </div>

//       {/* Field */}
//       <div>
//         <Label>Field of Study</Label>
//         <Input
//           value={formData.field}
//           onChange={(e) => handleInputChange('field', e.target.value)}
//         />
//         {errors.field && <p className="text-red-500 text-sm">{errors.field}</p>}
//       </div>

//       {/* ...rest of form, you can similarly show errors.startDate / errors.endDate */}
//       <div className="flex gap-2">
//         <Button onClick={handleSave}>
//           {educationToEdit ? 'Update' : 'Save'}
//         </Button>
//         <Button variant="outline" onClick={handleCancel}>
//           Cancel
//         </Button>
//       </div>
//     </div>
//   );

//   // ...existing render code...
// }
