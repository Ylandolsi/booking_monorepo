import { UserDataWrapper } from '@/features/auth';
import { Expertise } from './expertise/expertise';
import { Experience } from './experience/experience';
import { Education } from './education/education';

export function Career() {
  return (
    <UserDataWrapper>
      {(userData) => (
        <div className="space-y-4">
          <Expertise userData={userData} />
          <Experience userData={userData} />
          <Education userData={userData} />
        </div>
      )}
    </UserDataWrapper>
  );
}
