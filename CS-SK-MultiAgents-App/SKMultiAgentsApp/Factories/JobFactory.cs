namespace SKMultiAgentsApp.Factories;

public static class JobFactory
{
    public static IEnumerable<JobInfo> CreateJobs()
    {
        var jobListings = new List<JobInfo>
        {
            new()
            {
                Title = "Fullstack .NET Developer",
                Company = "TechSolutions Inc.",
                Location = "Seattle, WA",
                Description = "Join our team to develop scalable web applications using .NET Core, C#, and React.",
                RequiredSkills = ["C#", ".NET Core", "React", "SQL Server", "Azure"],
                SalaryRange = "$90,000 - $120,000",
                ApplicationUrl = "https://techsolutions.com/careers/fullstack-dotnet"
            },
            new()
            {
                Title = "C# Team Lead Developer",
                Company = "Enterprise Systems Group",
                Location = "Boston, MA",
                Description = "Lead a team of developers building enterprise-level applications with C# and .NET technologies.",
                RequiredSkills = ["C#", ".NET Framework", "Team Leadership", "Microservices", "CI/CD"],
                SalaryRange = "$130,000 - $160,000",
                ApplicationUrl = "https://esg.com/jobs/team-lead"
            },
            new()
            {
                Title = "Junior C# Developer",
                Company = "CodeCraft Software",
                Location = "Austin, TX",
                Description = "Great opportunity for developers starting their career in .NET development.",
                RequiredSkills = ["C#", ".NET", "HTML/CSS", "JavaScript", "Git"],
                SalaryRange = "$65,000 - $85,000",
                ApplicationUrl = "https://codecraft.io/apply/junior-csharp"
            },
            new()
            {
                Title = "Senior Java Developer",
                Company = "FinTech Solutions",
                Location = "New York, NY",
                Description = "Build high-performance financial software applications using Java and Spring.",
                RequiredSkills = ["Java", "Spring Boot", "Hibernate", "PostgreSQL", "Microservices"],
                SalaryRange = "$120,000 - $150,000",
                ApplicationUrl = "https://fintechsolutions.com/careers/java-dev"
            },
            new()
            {
                Title = "Frontend React Developer",
                Company = "UX Innovations",
                Location = "San Francisco, CA",
                Description = "Create beautiful, responsive user interfaces for our SaaS products.",
                RequiredSkills = ["React", "TypeScript", "Redux", "CSS-in-JS", "Jest"],
                SalaryRange = "$95,000 - $130,000",
                ApplicationUrl = "https://uxinnovations.com/jobs/frontend"
            },
            new()
            {
                Title = "DevOps Engineer",
                Company = "Cloud Systems Inc.",
                Location = "Denver, CO",
                Description = "Manage and optimize our cloud infrastructure and CI/CD pipelines.",
                RequiredSkills = ["AWS", "Kubernetes", "Docker", "Terraform", "Jenkins"],
                SalaryRange = "$110,000 - $140,000",
                ApplicationUrl = "https://cloudsystemsinc.com/devops-position"
            },
            new()
            {
                Title = "Data Scientist",
                Company = "Analytics Pro",
                Location = "Chicago, IL",
                Description = "Apply machine learning techniques to derive insights from large datasets.",
                RequiredSkills = ["Python", "TensorFlow", "SQL", "Data Visualization", "Statistics"],
                SalaryRange = "$100,000 - $135,000",
                ApplicationUrl = "https://analyticspro.com/careers/datascientist"
            },
            new()
            {
                Title = "Mobile App Developer",
                Company = "AppWorks Mobile",
                Location = "Portland, OR",
                Description = "Create cross-platform mobile applications using modern frameworks.",
                RequiredSkills = ["React Native", "Flutter", "iOS", "Android", "API Integration"],
                SalaryRange = "$85,000 - $120,000",
                ApplicationUrl = "https://appworks.dev/jobs/mobile-developer"
            },
            new()
            {
                Title = "Registered Nurse",
                Company = "Memorial Hospital",
                Location = "Phoenix, AZ",
                Description = "Provide patient care in our busy medical-surgical unit.",
                RequiredSkills =
                [
                    "RN License", "BLS Certification", "Patient Assessment", "Electronic Medical Records",
                    "Care Planning"
                ],
                SalaryRange = "$75,000 - $95,000",
                ApplicationUrl = "https://memorialhospital.org/careers/nursing"
            },
            new()
            {
                Title = "Medical Laboratory Technician",
                Company = "Diagnostic Labs Inc.",
                Location = "Minneapolis, MN",
                Description = "Perform various laboratory tests on patient samples to aid in diagnosis and treatment.",
                RequiredSkills =
                    ["MLT Certification", "Specimen Processing", "Lab Equipment", "Quality Control", "Data Entry"],
                SalaryRange = "$55,000 - $70,000",
                ApplicationUrl = "https://diagnosticlabs.com/jobs/lab-tech"
            },
            new()
            {
                Title = "Financial Analyst",
                Company = "Global Investments LLC",
                Location = "Charlotte, NC",
                Description = "Analyze financial data and provide recommendations for investment strategies.",
                RequiredSkills =
                    ["Financial Modeling", "Excel", "Bloomberg Terminal", "Financial Statements", "Forecasting"],
                SalaryRange = "$85,000 - $110,000",
                ApplicationUrl = "https://globalinvestments.com/careers/financial-analyst"
            },
            new()
            {
                Title = "Tax Accountant",
                Company = "Precision Tax Services",
                Location = "Dallas, TX",
                Description = "Prepare and file tax returns for businesses and high-net-worth individuals.",
                RequiredSkills = ["CPA", "Tax Law", "Tax Software", "Financial Analysis", "Client Communication"],
                SalaryRange = "$70,000 - $95,000",
                ApplicationUrl = "https://precisiontax.com/openings/accountant"
            },
            new()
            {
                Title = "High School Math Teacher",
                Company = "Westview Public Schools",
                Location = "Sacramento, CA",
                Description = "Teach algebra, geometry, and calculus to students in grades 9-12.",
                RequiredSkills =
                [
                    "Teaching Credential", "Mathematics Degree", "Curriculum Development", "Classroom Management",
                    "Student Assessment"
                ],
                SalaryRange = "$60,000 - $80,000",
                ApplicationUrl = "https://westview.edu/employment/math-teacher"
            },
            new()
            {
                Title = "ESL Instructor",
                Company = "International Language Institute",
                Location = "Miami, FL",
                Description = "Teach English as a second language to adult learners from diverse backgrounds.",
                RequiredSkills =
                [
                    "TESOL Certification", "Second Language", "Lesson Planning", "Cultural Sensitivity",
                    "Assessment Design"
                ],
                SalaryRange = "$50,000 - $65,000",
                ApplicationUrl = "https://ili.edu/careers/esl-instructor"
            },
            new()
            {
                Title = "Digital Marketing Manager",
                Company = "BrandBoost Agency",
                Location = "Atlanta, GA",
                Description = "Develop and implement digital marketing strategies across multiple channels.",
                RequiredSkills =
                    ["SEO/SEM", "Social Media Marketing", "Google Analytics", "Content Strategy", "Email Marketing"],
                SalaryRange = "$80,000 - $110,000",
                ApplicationUrl = "https://brandboost.com/join/marketing-manager"
            },
            new()
            {
                Title = "Brand Strategist",
                Company = "Creative Minds Marketing",
                Location = "Los Angeles, CA",
                Description = "Create compelling brand narratives and positioning for our clients.",
                RequiredSkills =
                [
                    "Market Research", "Brand Development", "Competitive Analysis", "Storytelling",
                    "Presentation Skills"
                ],
                SalaryRange = "$75,000 - $100,000",
                ApplicationUrl = "https://creativemindsmktg.com/careers/strategist"
            },
            new()
            {
                Title = "Executive Chef",
                Company = "Grand Hotel & Resort",
                Location = "Las Vegas, NV",
                Description = "Lead our culinary team in creating exceptional dining experiences for our guests.",
                RequiredSkills =
                    ["Culinary Degree", "Menu Development", "Kitchen Management", "Food Safety", "Inventory Control"],
                SalaryRange = "$75,000 - $110,000",
                ApplicationUrl = "https://grandhotel.com/jobs/executive-chef"
            },
            new()
            {
                Title = "Event Coordinator",
                Company = "Celebration Venues",
                Location = "Nashville, TN",
                Description = "Plan and execute flawless events from weddings to corporate gatherings.",
                RequiredSkills =
                [
                    "Event Planning", "Vendor Management", "Budget Administration", "Customer Service",
                    "Problem Solving"
                ],
                SalaryRange = "$50,000 - $70,000",
                ApplicationUrl = "https://celebrationvenues.com/careers/coordinator"
            }
        };
        return jobListings;
    }
}