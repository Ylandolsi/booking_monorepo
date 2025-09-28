using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Booking.Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAndRemoveDepricatedFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "educations",
                schema: "users");

            migrationBuilder.DropTable(
                name: "experiences",
                schema: "users");

            migrationBuilder.DropTable(
                name: "user_expertises",
                schema: "users");

            migrationBuilder.DropTable(
                name: "user_languages",
                schema: "users");

            migrationBuilder.DropTable(
                name: "user_mentors",
                schema: "users");

            migrationBuilder.DropTable(
                name: "expertises",
                schema: "users");

            migrationBuilder.DropTable(
                name: "languages",
                schema: "users");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_completion_status",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_bio",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_connected_with_google",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_education",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_experience",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_expertise",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_languages",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_profile_picture",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_has_social_links",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_completion_status_total_fields",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_picture_url_profile_picture_link",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "profile_picture_url_thumbnail_url_picture_link",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "social_links_facebook",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "social_links_github",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "social_links_instagram",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "social_links_linked_in",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "social_links_portfolio",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "social_links_twitter",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "social_links_youtube",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "status_is_active",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "status_is_mentor",
                schema: "users",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "profile_completion_status_completion_status",
                schema: "users",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_bio",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_connected_with_google",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_education",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_experience",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_expertise",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_languages",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_profile_picture",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "profile_completion_status_has_social_links",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "profile_completion_status_total_fields",
                schema: "users",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "profile_picture_url_profile_picture_link",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "profile_picture_url_thumbnail_url_picture_link",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "social_links_facebook",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "social_links_github",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "social_links_instagram",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "social_links_linked_in",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "social_links_portfolio",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "social_links_twitter",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "social_links_youtube",
                schema: "users",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "status_is_active",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "status_is_mentor",
                schema: "users",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "educations",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    field = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    to_present = table.Column<bool>(type: "boolean", nullable: false),
                    university = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_educations", x => x.id);
                    table.CheckConstraint("CK_Education_Dates_Valid", "end_date IS NULL OR end_date >= start_date");
                    table.CheckConstraint("CK_Education_Field_Length", "LENGTH(field) >= 2 AND LENGTH(field) <= 100");
                    table.CheckConstraint("CK_Education_University_Length", "LENGTH(university) >= 2 AND LENGTH(university) <= 100");
                    table.ForeignKey(
                        name: "fk_educations_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "experiences",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    company = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    to_present = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_experiences", x => x.id);
                    table.CheckConstraint("CK_Experience_Company_Length", "LENGTH(company) >= 2 AND LENGTH(company) <= 100");
                    table.CheckConstraint("CK_Experience_Dates_Valid", "end_date IS NULL OR end_date >= start_date");
                    table.CheckConstraint("CK_Experience_Title_Length", "LENGTH(title) >= 2 AND LENGTH(title) <= 100");
                    table.ForeignKey(
                        name: "fk_experiences_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expertises",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_expertises", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                schema: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_mentors",
                schema: "users",
                columns: table => new
                {
                    mentor_id = table.Column<int>(type: "integer", nullable: false),
                    mentee_id = table.Column<int>(type: "integer", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_mentors", x => new { x.mentor_id, x.mentee_id });
                    table.ForeignKey(
                        name: "fk_user_mentors_asp_net_users_mentee_id",
                        column: x => x.mentee_id,
                        principalSchema: "users",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_mentors_asp_net_users_mentor_id",
                        column: x => x.mentor_id,
                        principalSchema: "users",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_expertises",
                schema: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    expertise_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_expertises", x => new { x.user_id, x.expertise_id });
                    table.ForeignKey(
                        name: "fk_user_expertises_expertises_expertise_id",
                        column: x => x.expertise_id,
                        principalSchema: "users",
                        principalTable: "expertises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_expertises_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_languages",
                schema: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_languages", x => new { x.user_id, x.language_id });
                    table.ForeignKey(
                        name: "fk_user_languages_languages_language_id",
                        column: x => x.language_id,
                        principalSchema: "users",
                        principalTable: "languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_languages_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "users",
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "expertises",
                columns: new[] { "id", "created_at", "description", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(827), "Mentorship in web, backend, mobile, etc.", "Software Engineering", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(4) },
                    { 2, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1153), "Mentorship in machines, manufacturing, CAD, etc.", "Mechanical Engineering", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1150) },
                    { 3, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1155), "Mentorship in circuits, power systems, etc.", "Electrical Engineering", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1154) },
                    { 4, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1157), "Mentorship in construction, infrastructure, etc.", "Civil Engineering", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1156) },
                    { 5, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1159), "Mentorship in chemical processes, materials, etc.", "Chemical Engineering", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1158) },
                    { 6, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1171), "Mentorship in aircraft, spacecraft, etc.", "Aerospace Engineering", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1170) },
                    { 7, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1172), "Mentorship in sustainability, environment, etc.", "Environmental Engineering", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1172) },
                    { 8, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1174), "Frontend, backend, and fullstack web mentoring.", "Web Development", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1174) },
                    { 9, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1176), "Android, iOS, cross-platform app mentoring.", "Mobile Development", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1175) },
                    { 10, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1179), "Mentorship in data analysis, ML, statistics.", "Data Science", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1178) },
                    { 11, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1181), "Mentorship in ethical hacking, defense, etc.", "Cybersecurity", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1180) },
                    { 12, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1183), "AWS, Azure, CI/CD, and infrastructure mentoring.", "Cloud & DevOps", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1182) },
                    { 13, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1184), "Mentorship in ML models, AI theory, etc.", "AI & Machine Learning", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1184) },
                    { 14, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1186), "Mentorship in user experience and interface design.", "UI/UX Design", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1186) },
                    { 15, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1188), "Mentorship for startup founders and entrepreneurs.", "Startup Coaching", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1187) },
                    { 16, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1190), "Mentorship in business models and scaling.", "Business Strategy", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1189) },
                    { 17, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1192), "Mentorship in digital marketing, social media, etc.", "Marketing & Branding", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1191) },
                    { 18, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1195), "Mentorship in B2B, B2C, pitching, etc.", "Sales", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1194) },
                    { 19, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1197), "Mentorship in online business and marketplaces.", "E-commerce", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1196) },
                    { 20, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1198), "Mentorship in product lifecycle, agile, etc.", "Product Management", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1198) },
                    { 21, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1200), "Mentorship in managing teams and tasks.", "Project Management", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1200) },
                    { 22, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1202), "Mentorship in stocks, real estate, etc.", "Investment", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1201) },
                    { 23, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1204), "Budgeting, saving, financial planning.", "Personal Finance", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1203) },
                    { 24, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1206), "Corporate and freelance financial help.", "Accounting & Auditing", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1205) },
                    { 25, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1207), "Medical school and residency mentorship.", "General Medicine", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1207) },
                    { 26, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1209), "Clinical mentorship and nursing school support.", "Nursing", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1209) },
                    { 27, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1211), "Pharmaceutical career and education guidance.", "Pharmacy", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1210) },
                    { 28, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1213), "Psychology, therapy, and emotional support.", "Mental Health", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1212) },
                    { 29, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1214), "Mentorship in epidemiology, policy, etc.", "Public Health", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1214) },
                    { 30, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1216), "Mentorship in contracts, companies, etc.", "Corporate Law", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1216) },
                    { 31, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1218), "Legal career coaching in criminal justice.", "Criminal Law", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1217) },
                    { 32, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1220), "Mentorship in visa and immigration processes.", "Immigration Law", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1219) },
                    { 33, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1222), "Mentorship in patents, copyrights, etc.", "Intellectual Property Law", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1221) },
                    { 34, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1225), "Mentorship in team building and leading.", "Leadership & Management", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1225) },
                    { 35, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1227), "Mentorship in effective communication.", "Communication Skills", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1226) },
                    { 36, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1229), "Coaching on personal productivity.", "Time Management", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1228) },
                    { 37, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1230), "Confidence building and speech coaching.", "Public Speaking", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1230) },
                    { 38, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1232), "Mock interviews and job prep.", "Job Interview Coaching", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1232) },
                    { 39, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1234), "Profile and resume optimization.", "Resume & LinkedIn Review", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1233) },
                    { 40, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1236), "Long-term goal mentorship.", "Career Planning", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1235) },
                    { 41, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1238), "Mentorship in tools like Photoshop, Figma.", "Graphic Design", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1237) },
                    { 42, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1239), "Camera use, editing, and career advice.", "Photography", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1239) },
                    { 43, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1241), "Mentorship in composition, mixing, etc.", "Music Production", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1241) },
                    { 44, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1243), "Mentorship in writing books or articles.", "Writing & Publishing", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1242) },
                    { 45, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1245), "Mentorship in Premiere Pro, storytelling, etc.", "Video Editing", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1244) },
                    { 46, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1246), "Unity, Unreal, and career mentorship.", "Game Development", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1246) },
                    { 47, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1248), "Mentorship for language fluency.", "Language Learning", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1248) },
                    { 48, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1250), "Mentorship for international students.", "Study Abroad Guidance", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1249) },
                    { 49, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1252), "Mentorship in motivation, habits, etc.", "Life Coaching", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1251) },
                    { 50, new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1254), "System building, discipline, deep work.", "Productivity Coaching", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(1253) }
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "languages",
                columns: new[] { "id", "created_at", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 25, 8, 25, 35, 180, DateTimeKind.Utc).AddTicks(725), "English", new DateTime(2025, 9, 25, 8, 25, 35, 179, DateTimeKind.Utc).AddTicks(9888) },
                    { 2, new DateTime(2025, 9, 25, 8, 25, 35, 180, DateTimeKind.Utc).AddTicks(754), "French", new DateTime(2025, 9, 25, 8, 25, 35, 180, DateTimeKind.Utc).AddTicks(752) },
                    { 3, new DateTime(2025, 9, 25, 8, 25, 35, 180, DateTimeKind.Utc).AddTicks(756), "Arabic", new DateTime(2025, 9, 25, 8, 25, 35, 180, DateTimeKind.Utc).AddTicks(755) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_educations_start_date",
                schema: "users",
                table: "educations",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "ix_educations_user_id",
                schema: "users",
                table: "educations",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_educations_user_id_start_date",
                schema: "users",
                table: "educations",
                columns: new[] { "user_id", "start_date" });

            migrationBuilder.CreateIndex(
                name: "ix_experiences_start_date",
                schema: "users",
                table: "experiences",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "ix_experiences_user_id",
                schema: "users",
                table: "experiences",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_experiences_user_id_start_date",
                schema: "users",
                table: "experiences",
                columns: new[] { "user_id", "start_date" });

            migrationBuilder.CreateIndex(
                name: "ix_user_expertises_expertise_id",
                schema: "users",
                table: "user_expertises",
                column: "expertise_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_languages_language_id",
                schema: "users",
                table: "user_languages",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_mentors_mentee_id",
                schema: "users",
                table: "user_mentors",
                column: "mentee_id");
        }
    }
}
