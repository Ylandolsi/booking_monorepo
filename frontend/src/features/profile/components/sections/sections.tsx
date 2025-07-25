import { useState } from 'react';
import { Overview } from './overview';

type sectonsType = 'Overview' | 'Mentors' | 'Calendar' ;

export function Sections() {
  let sections = ['Overview', 'Mentors' , 'Calendar'];
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
