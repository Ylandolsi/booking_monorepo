import { Expertise } from './expertise/expertise';
import { Experience } from './experience/experience';
import { Education } from './education/education';
import { QueryWrapper } from '@/components';
import { useUser } from '@/api/auth';

export function Career() {
  const userQuery = useUser();
  return (
    <QueryWrapper query={userQuery}>
      {(userData) => (
        <div className="space-y-4">
          <Expertise userData={userData} />
          <Experience userData={userData} />
          <Education userData={userData} />
        </div>
      )}
    </QueryWrapper>
  );
}
