using Booking.Modules.Users.Domain.Entities;

namespace Booking.Modules.Users.Presistence.Seed.Users;

public static class LanguageExpertiseSeeder
{
    public static List<Language> Languages()
    {
        var languages = new List<Language>
        {
            new("English", 1),
            new("French", 2),
            new("Arabic", 3)
        };
        return languages;
    }

    public static List<Expertise> Expertises()
    {
        var expertises = new List<Expertise>
        {
            new("Software Engineering", "Mentorship in web, backend, mobile, etc.", 1),
            new("Mechanical Engineering", "Mentorship in machines, manufacturing, CAD, etc.", 2),
            new("Electrical Engineering", "Mentorship in circuits, power systems, etc.", 3),
            new("Civil Engineering", "Mentorship in construction, infrastructure, etc.", 4),
            new("Chemical Engineering", "Mentorship in chemical processes, materials, etc.", 5),
            new("Aerospace Engineering", "Mentorship in aircraft, spacecraft, etc.", 6),
            new("Environmental Engineering", "Mentorship in sustainability, environment, etc.", 7),

            new("Web Development", "Frontend, backend, and fullstack web mentoring.", 8),
            new("Mobile Development", "Android, iOS, cross-platform app mentoring.", 9),
            new("Data Science", "Mentorship in data analysis, ML, statistics.", 10),
            new("Cybersecurity", "Mentorship in ethical hacking, defense, etc.", 11),
            new("Cloud & DevOps", "AWS, Azure, CI/CD, and infrastructure mentoring.", 12),
            new("AI & Machine Learning", "Mentorship in ML models, AI theory, etc.", 13),
            new("UI/UX Design", "Mentorship in user experience and interface design.", 14),

            new("Startup Coaching", "Mentorship for startup founders and entrepreneurs.", 15),
            new("Business Strategy", "Mentorship in business models and scaling.", 16),
            new("Marketing & Branding", "Mentorship in digital marketing, social media, etc.", 17),
            new("Sales", "Mentorship in B2B, B2C, pitching, etc.", 18),
            new("E-commerce", "Mentorship in online business and marketplaces.", 19),
            new("Product Management", "Mentorship in product lifecycle, agile, etc.", 20),
            new("Project Management", "Mentorship in managing teams and tasks.", 21),

            new("Investment", "Mentorship in stocks, real estate, etc.", 22),
            new("Personal Finance", "Budgeting, saving, financial planning.", 23),
            new("Accounting & Auditing", "Corporate and freelance financial help.", 24),

            new("General Medicine", "Medical school and residency mentorship.", 25),
            new("Nursing", "Clinical mentorship and nursing school support.", 26),
            new("Pharmacy", "Pharmaceutical career and education guidance.", 27),
            new("Mental Health", "Psychology, therapy, and emotional support.", 28),
            new("Public Health", "Mentorship in epidemiology, policy, etc.", 29),

            new("Corporate Law", "Mentorship in contracts, companies, etc.", 30),
            new("Criminal Law", "Legal career coaching in criminal justice.", 31),
            new("Immigration Law", "Mentorship in visa and immigration processes.", 32),
            new("Intellectual Property Law", "Mentorship in patents, copyrights, etc.", 33),

            new("Leadership & Management", "Mentorship in team building and leading.", 34),
            new("Communication Skills", "Mentorship in effective communication.", 35),
            new("Time Management", "Coaching on personal productivity.", 36),
            new("Public Speaking", "Confidence building and speech coaching.", 37),
            new("Job Interview Coaching", "Mock interviews and job prep.", 38),
            new("Resume & LinkedIn Review", "Profile and resume optimization.", 39),
            new("Career Planning", "Long-term goal mentorship.", 40),

            new("Graphic Design", "Mentorship in tools like Photoshop, Figma.", 41),
            new("Photography", "Camera use, editing, and career advice.", 42),
            new("Music Production", "Mentorship in composition, mixing, etc.", 43),
            new("Writing & Publishing", "Mentorship in writing books or articles.", 44),
            new("Video Editing", "Mentorship in Premiere Pro, storytelling, etc.", 45),
            new("Game Development", "Unity, Unreal, and career mentorship.", 46),
            new("Language Learning", "Mentorship for language fluency.", 47),
            new("Study Abroad Guidance", "Mentorship for international students.", 48),
            new("Life Coaching", "Mentorship in motivation, habits, etc.", 49),
            new("Productivity Coaching", "System building, discipline, deep work.", 50)
        };
        return expertises;
    }
}