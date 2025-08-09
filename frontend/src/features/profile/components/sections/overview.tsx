import { MainErrorFallback, SocialLinks } from '@/components';
import { MdEdit, MdOutlineWork } from 'react-icons/md';
import { FaBookReader } from 'react-icons/fa';
import {
  useProfileBySlug,
  type ExpertiseType,
  type LanguageType,
} from '@/features/profile';
import { Button, PageLoading } from '@/components/ui';
import { useRequiredParam } from '@/hooks';
import { formatDate } from '@/utils';
import { useProfileEditStore } from '@/features/profile';

export function Overview() {
  const userSlug = useRequiredParam('userSlug');
  const { openDialog, setDefaultSection } = useProfileEditStore();

  const { user, error, isLoading, isSlugCurrent } = useProfileBySlug();

  if (userSlug == undefined || userSlug == '') return null;

  if (error) return <MainErrorFallback />;
  if (isLoading) return <PageLoading />;
  if (!user) return null;

  return (
    <div className="p-6 sm:p-8 max-w-4xl mx-auto">
      <div className="space-y-8">
        {/* Bio Section */}
        {user.bio && (
          <div className="bg-card rounded-xl p-6 shadow-sm border border-border/50">
            <h2 className="text-xl font-semibold mb-3 text-foreground">
              About
            </h2>
            <p className="text-muted-foreground leading-relaxed">{user.bio}</p>
          </div>
        )}
        {/* Social Links */}
        {user?.socialLinks && Object.keys(user.socialLinks).length > 0 && (
          <div className="bg-card rounded-xl p-6 shadow-sm border border-border/50">
            <h2 className="text-xl font-semibold mb-4 text-foreground">
              Social Links
            </h2>
            <SocialLinks links={user.socialLinks ?? {}} variant="icons" />
          </div>
        )}

        {/* Expertise Section */}
        {user.expertises?.length > 0 && (
          <div className="bg-card rounded-xl p-6 shadow-sm border border-border/50">
            <h2 className="text-xl font-semibold mb-4 text-foreground">
              Expertise
            </h2>
            <div className="flex flex-wrap gap-3">
              {user.expertises.map(
                (expertise: ExpertiseType, index: number) => (
                  <span
                    key={expertise.id || index}
                    className="px-4 py-2 bg-primary/10 text-primary rounded-full text-sm font-medium"
                  >
                    {expertise.name}
                  </span>
                ),
              )}
            </div>
          </div>
        )}

        {/* Languages Section */}
        {user.languages?.length > 0 && (
          <div className="bg-card rounded-xl p-6 shadow-sm border border-border/50">
            <div className="flex justify-between">
              <h2 className="text-xl font-semibold mb-4 text-foreground">
                Languages
              </h2>
              {isSlugCurrent && (
                <Button
                  onClick={() => {
                    setDefaultSection('Basic Info');
                    openDialog();
                  }}
                  className="p-2 rounded-full bg-primary/10 hover:bg-primary/20 transition-colors"
                >
                  <MdEdit className="text-primary w-4 h-4" />
                </Button>
              )}
            </div>

            <div className="flex flex-wrap gap-3">
              {user.languages.map((language: LanguageType, index: number) => (
                <span
                  key={language.id || index}
                  className="px-4 py-2 bg-blue-100 text-blue-600 rounded-full text-sm font-medium"
                >
                  {language.name}
                </span>
              ))}
            </div>
          </div>
        )}

        {/* Experience Section */}
        {user.experiences?.length > 0 && (
          <div className="bg-card rounded-xl p-6 shadow-sm border border-border/50">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-xl font-semibold text-foreground">
                Experience
              </h2>
              {isSlugCurrent && (
                <Button
                  onClick={() => {
                    setDefaultSection('Career');
                    openDialog();
                  }}
                  className="p-2 rounded-full bg-primary/10 hover:bg-primary/20 transition-colors"
                >
                  <MdEdit className="text-primary w-4 h-4" />
                </Button>
              )}
            </div>
            <div className="space-y-4">
              {user.experiences.map((experience, index) => (
                <div key={experience.id || index} className="flex gap-4">
                  <div className="flex-shrink-0 w-12 h-12 bg-primary/10 rounded-xl flex items-center justify-center">
                    <MdOutlineWork className="w-6 h-6 text-primary" />
                  </div>
                  <div className="flex-1">
                    <h3 className="font-semibold text-foreground mb-1">
                      {experience.title}
                    </h3>
                    <p className="text-muted-foreground text-sm mb-2">
                      {experience.company}
                    </p>
                    <p className="text-xs text-muted-foreground">
                      {formatDate(experience.startDate)} -{' '}
                      {experience.endDate
                        ? formatDate(experience.endDate)
                        : 'Present'}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* Education Section */}
        {user.educations?.length > 0 && (
          <div className="bg-card rounded-xl p-6 shadow-sm border border-border/50">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-xl font-semibold text-foreground">
                Education
              </h2>
              {isSlugCurrent && (
                <Button
                  onClick={() => {
                    setDefaultSection('Career');
                    openDialog();
                  }}
                  className="p-2 rounded-full bg-primary/10 hover:bg-primary/20 transition-colors"
                >
                  <MdEdit className="text-primary w-4 h-4" />
                </Button>
              )}
            </div>
            <div className="space-y-4">
              {user.educations.map((education, index) => (
                <div key={education.id || index} className="flex gap-4">
                  <div className="flex-shrink-0 w-12 h-12 bg-blue-50 rounded-xl flex items-center justify-center">
                    <FaBookReader className="w-6 h-6 text-blue-600" />
                  </div>
                  <div className="flex-1">
                    <h3 className="font-semibold text-foreground mb-1">
                      {education.field}
                    </h3>
                    <p className="text-muted-foreground text-sm mb-2">
                      {education.university}
                    </p>
                    <p className="text-xs text-muted-foreground">
                      {formatDate(education.startDate)} -{' '}
                      {education.endDate
                        ? formatDate(education.endDate)
                        : 'Present'}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
