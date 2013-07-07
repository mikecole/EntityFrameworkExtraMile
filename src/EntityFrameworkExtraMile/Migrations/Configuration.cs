using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using EntityFrameworkExtraMile.Domain.Model;
using EntityFrameworkExtraMile.Infrastructure.DataAccess;
using WebMatrix.WebData;

namespace EntityFrameworkExtraMile.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<HumanResourceContext>
    {
        private Random _random = new Random();
        
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(HumanResourceContext context)
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfiles", "ID", "UserName", true);
            }

            SeedState(context, new[] { "AL", "Alabama" }, new[] { "AK", "Alaska" }, new[] { "AZ", "Arizona" }, new[] { "AR", "Arkansas" }, new[] { "CA", "California" }, new[] { "CO", "Colorado" }, new[] { "CT", "Connecticut" }, new[] { "DE", "Delaware" }, new[] { "DC", "District of Columbia" }, new[] { "FL", "Florida" }, new[] { "GA", "Georgia" }, new[] { "HI", "Hawaii" }, new[] { "ID", "Idaho" }, new[] { "IL", "Illinois" }, new[] { "IN", "Indiana" }, new[] { "IA", "Iowa" }, new[] { "KS", "Kansas" }, new[] { "KY", "Kentucky" }, new[] { "LA", "Louisiana" }, new[] { "ME", "Maine" }, new[] { "MD", "Maryland" }, new[] { "MA", "Massachusetts" }, new[] { "MI", "Michigan" }, new[] { "MN", "Minnesota" }, new[] { "MS", "Mississippi" }, new[] { "MO", "Missouri" }, new[] { "MT", "Montana" }, new[] { "NE", "Nebraska" }, new[] { "NV", "Nevada" }, new[] { "NH", "New Hampshire" }, new[] { "NJ", "New Jersey" }, new[] { "NM", "New Mexico" }, new[] { "NY", "New York" }, new[] { "NC", "North Carolina" }, new[] { "ND", "North Dakota" }, new[] { "OH", "Ohio" }, new[] { "OK", "Oklahoma" }, new[] { "OR", "Oregon" }, new[] { "PA", "Pennsylvania" }, new[] { "RI", "Rhode Island" }, new[] { "SC", "South Carolina" }, new[] { "SD", "South Dakota" }, new[] { "TN", "Tennessee" }, new[] { "TX", "Texas" }, new[] { "UT", "Utah" }, new[] { "VT", "Vermont" }, new[] { "VA", "Virginia" }, new[] { "WA", "Washington" }, new[] { "WV", "West Virginia" }, new[] { "WI", "Wisconsin" }, new[] { "WY", "Wyoming" });
            SeedDepartments(context, new[] { "IT", "Information Technologies" }, new[] { "SALES", "Sales" }, new[] { "ACCT", "Accounting" }, new[] { "R&D", "Research and Development" }, new[] { "HR", "Human Resources" }, new[] { "SEC", "Security" }, new[] { "MARK", "Marketing" }, new[] { "FAC", "Facilities" }, new[] { "EXEC", "Executive" }, new[] { "WH", "Warehouse" });
            SeedPayrollDeductions(context, new[] { "YMCA Single Membership", "19.99" }, new[] { "YMCA Family Membership", "29.99" }, new[] { "Dependent Day Care", "89.99" }, new[] { "Employee Cell Phone", "49.99" }, new[] { "Public Transportation Pass", "49.99" });
            SeedEmployees(context);
        }

        private void SeedState(HumanResourceContext context, params string[][] states)
        {
            foreach (var item in states)
            {
                context.States.AddOrUpdate(s => s.Abbreviation, new State(item[0], item[1]));
                context.SaveChanges();
            }
        }

        private void SeedDepartments(HumanResourceContext context, params string[][] departments)
        {
            foreach (var item in departments)
            {
                context.Departments.AddOrUpdate(d => d.Code, new Department(item[0], item[1]));
                context.SaveChanges();
            }
        }

        private void SeedPayrollDeductions(HumanResourceContext context, params string[][] deductions)
        {
            foreach (var item in deductions)
            {
                context.PayrollDeductions.AddOrUpdate(s => s.Name, new PayrollDeduction { Amount = Decimal.Parse(item[1]), Name = item[0] });
                context.SaveChanges();
            }
        }

        private void SeedEmployees(HumanResourceContext context)
        {
            var employees = GetEmployeeData().Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            var states = context.States.ToArray();
            var departments = context.Departments.ToArray();
            var payrollDeductions = context.PayrollDeductions.ToArray();

            foreach (var employee in employees)
            {
                var fields = employee.Split('\t');

                context.Employees.AddOrUpdate(e => e.Code, new Employee
                    {
                        Code = fields[0],
                        FirstName = fields[1],
                        MiddleName = fields[2],
                        LastName = fields[3],
                        Gender = fields[4] == "M" ? Genders.Male : Genders.Female,
                        Address = new Address
                            {
                                AddressLine1 = fields[5],
                                AddressLine2 = fields[6] != "NULL" ? fields[6] : null,
                                City = fields[7],
                                State = states.SingleOrDefault(st => st.Name == fields[9]),
                                PostalCode = fields[10]
                            },
                        DateOfBirth = DateTime.Parse(fields[11]),
                        HireDate = DateTime.Parse(fields[12]),
                        Department = departments.ElementAt(Int32.Parse(fields[13])),
                        PayrollDeductions = GetRandomPayrollDeductions(payrollDeductions)
                    });

                context.SaveChanges();
            }
        }

        private ICollection<PayrollDeduction> GetRandomPayrollDeductions(IEnumerable<PayrollDeduction> payrollDeductions)
        {
            var result = new List<PayrollDeduction>();

            foreach (var deduction in payrollDeductions)
            {
                if ((_random.Next(1, 3)%2 == 0))
                {
                    result.Add(deduction);
                }
            }

            return result;
        }

        private string GetEmployeeData()
        {
            return @"TD'Hers	Thierry	B	D'Hers	M	1970 Napa Ct.	NULL	Bothell	79	Washington	98011	1953-08-29	2002-01-11	7
VKuppa	Vamsi	N	Kuppa	M	9833 Mt. Dias Blv.	NULL	Bothell	79	Washington	98011	1971-04-19	2003-01-08	6
SAbbas	Syed	E	Abbas	M	7484 Roundtree Drive	NULL	Bothell	79	Washington	98011	1969-02-11	2007-04-15	4
MSandberg	Mikael	Q	Sandberg	M	9539 Glenside Dr	NULL	Bothell	79	Washington	98011	1978-09-18	2003-03-14	1
KRalls	Kim	T	Ralls	F	1226 Shoe St.	NULL	Bothell	79	Washington	98011	1978-06-01	2003-01-27	4
BNewman	Belinda	M	Newman	F	1399 Firestone Drive	NULL	Bothell	79	Washington	98011	1963-10-19	2003-03-24	6
BStadick	Betsy	A	Stadick	F	5672 Hale Dr.	NULL	Bothell	79	Washington	98011	1961-01-17	2004-01-19	8
BHohman	Bob	N	Hohman	M	6387 Scenic Avenue	NULL	Bothell	79	Washington	98011	1973-09-16	2003-01-25	3
PWu	Peng	J	Wu	M	250 Race Court	NULL	Bothell	79	Washington	98011	1970-04-19	2003-01-10	5
BGoldstein	Brian	Richard	Goldstein	M	8157 W. Book	NULL	Bothell	79	Washington	98011	1965-01-23	2004-01-12	1
CPhilips	Carol	M	Philips	F	6872 Thornwood Dr.	NULL	Bothell	79	Washington	98011	1982-11-18	2003-03-16	7
CNiswonger	Chad	W	Niswonger	M	5747 Shirley Drive	NULL	Bothell	79	Washington	98011	1984-09-04	2003-03-22	6
PAnsman-Wolfe	Pamela	O	Ansman-Wolfe	F	636 Vine Hill Way	NULL	Portland	58	Oregon	97205	1969-01-06	2005-07-01	7
CPreston	Chris	T	Preston	M	6657 Sand Pointe Lane	NULL	Seattle	79	Washington	98104	1983-01-17	2003-02-23	2
JCarson	Jillian	NULL	Carson	F	80 Sunview Terrace	NULL	Duluth	36	Minnesota	55802	1956-09-29	2005-07-01	4
SIto	Shu	K	Ito	M	5725 Glaze Drive	NULL	San Francisco	9	California	94109	1962-04-10	2005-07-01	3
LMitchell	Linda	C	Mitchell	F	2487 Riverside Drive	NULL	Nevada	74	Utah	84407	1974-03-30	2005-07-01	4
TReiter	Tsvi	Michael	Reiter	M	8291 Crossbow Way	NULL	Memphis	72	Tennessee	38103	1968-02-19	2005-07-01	5
MBlythe	Michael	G	Blythe	M	8154 Via Mexico	NULL	Detroit	35	Michigan	48226	1963-01-26	2005-07-01	9
TMensa-Annan	Tete	A	Mensa-Annan	M	3997 Via De Luna	NULL	Cambridge	30	Massachusetts	02139	1972-02-06	2006-11-01	9
CHill	Christopher	E	Hill	M	1902 Santa Cruz	NULL	Bothell	79	Washington	98011	1980-11-01	2004-03-11	8
CRandall	Cynthia	S	Randall	F	463 H Stagecoach Rd.	NULL	Kenmore	79	Washington	98028	1975-09-19	2003-02-28	0
DTiedt	Danielle	C	Tiedt	F	5203 Virginia Lane	NULL	Kenmore	79	Washington	98028	1980-10-08	2004-03-23	6
DHamilton	David	P	Hamilton	M	4095 Cooper Dr.	NULL	Kenmore	79	Washington	98028	1977-08-02	2003-02-04	4
DJohnson	David	N	Johnson	M	6697 Ridge Park Drive	NULL	Kenmore	79	Washington	98028	1973-12-03	2003-01-03	9
DSmith	Denise	H	Smith	F	5669 Ironwood Way	NULL	Kenmore	79	Washington	98028	1982-08-07	2003-03-09	2
DTibbott	Diane	H	Tibbott	F	8192 Seagull Court	NULL	Kenmore	79	Washington	98028	1983-09-10	2003-02-19	4
DHartwig	Doris	M	Hartwig	F	5553 Cash Avenue	NULL	Kenmore	79	Washington	98028	1950-05-06	2002-04-11	8
DMiller	Dylan	A	Miller	M	7048 Laurel	NULL	Kenmore	79	Washington	98028	1981-03-27	2003-03-12	2
ABrewer	Alan	J	Brewer	M	25 95th Ave NE	NULL	Kenmore	79	Washington	98028	1978-04-30	2003-03-17	7
ACiccu	Alice	O	Ciccu	F	3280 Pheasant Circle	NULL	Snohomish	79	Washington	98296	1972-02-27	2003-01-08	5
BMartin	Benjamin	R	Martin	M	4231 Spar Court	NULL	Snohomish	79	Washington	98296	1980-02-06	2003-02-28	3
FFakhouri	Fadi	K	Fakhouri	M	1285 Greenbrier Street	NULL	Snohomish	79	Washington	98296	1983-03-19	2003-02-05	2
FMartinez	Frank	R	Martinez	M	5724 Victory Lane	NULL	Snohomish	79	Washington	98296	1946-04-03	2004-03-08	1
FMiller	Frank	T	Miller	M	591 Merriewood Drive	NULL	Snohomish	79	Washington	98296	1965-08-24	2003-03-27	4
BJohnson	Barry	K	Johnson	M	3114 Notre Dame Ave.	NULL	Snohomish	79	Washington	98296	1950-04-27	2002-02-07	0
BLloyd	Brian	T	Lloyd	M	7230 Vine Maple Street	NULL	Snohomish	79	Washington	98296	1971-03-14	2003-03-02	4
FNorthup	Fred	T	Northup	M	2601 Cambridge Drive	NULL	Snohomish	79	Washington	98296	1983-07-27	2003-01-13	4
GYoung	Garrett	R	Young	M	2115 Passing	NULL	Snohomish	79	Washington	98296	1978-09-26	2003-01-08	9
BKearney	Bonnie	N	Kearney	F	4852 Chaparral Court	NULL	Snohomish	79	Washington	98296	1980-10-11	2004-02-02	5
GGilbert	Guy	R	Gilbert	M	7726 Driftwood Drive	NULL	Monroe	79	Washington	98272	1976-05-15	2000-07-31	9
ISalmre	Ivo	William	Salmre	M	3841 Silver Oaks Place	NULL	Monroe	79	Washington	98272	1976-02-04	2003-01-05	4
JHamilton	James	R	Hamilton	M	9652 Los Angeles	NULL	Monroe	79	Washington	98272	1977-02-07	2003-03-07	3
JEsteves	Janeth	M	Esteves	F	4566 La Jolla	NULL	Monroe	79	Washington	98272	1966-08-25	2003-02-16	7
JRichins	Jack	S	Richins	M	1356 Grove Way	NULL	Monroe	79	Washington	98272	1977-07-23	2003-03-25	9
JKramer	James	D	Kramer	M	4734 Sycamore Court	NULL	Monroe	79	Washington	98272	1978-08-26	2003-01-28	8
JAdams	Jay	G	Adams	M	896 Southdale	NULL	Monroe	79	Washington	98272	1970-03-14	2003-04-06	5
BBaker	Bryan	NULL	Baker	M	2275 Valley Blvd.	NULL	Monroe	79	Washington	98272	1967-09-28	2003-02-22	9
MHines	Michael	T	Hines	M	1792 Belmont Rd.	NULL	Monroe	79	Washington	98272	1978-12-19	2003-01-10	8
JFord	Jeffrey	L	Ford	M	5734 Ashford Court	NULL	Monroe	79	Washington	98272	1950-08-12	2002-03-23	1
RCaron	Rob	T	Caron	M	5030 Blue Ridge Dr.	NULL	Monroe	79	Washington	98272	1967-09-05	2003-03-17	8
DLawrence	David	Oliver	Lawrence	M	158 Walnut Ave	NULL	Monroe	79	Washington	98272	1979-10-25	2003-03-18	9
JWang	Jian Shuo	NULL	Wang	M	8310 Ridge Circle	NULL	Monroe	79	Washington	98272	1977-08-27	2003-01-08	4
SDyck	Shelley	N	Dyck	F	3747 W. Landing Avenue	NULL	Monroe	79	Washington	98272	1981-01-08	2003-04-08	8
MSamant	Mandar	H	Samant	M	2598 La Vista Circle	NULL	Duvall	79	Washington	98019	1980-04-21	2003-03-14	6
JBrown	Jo	A	Brown	F	9693 Mellowood Street	NULL	Duvall	79	Washington	98019	1950-11-09	2002-03-30	7
LSteele	Laura	C	Steele	F	1825 Corte Del Prado	NULL	Duvall	79	Washington	98019	1975-01-26	2003-02-04	3
EBrown	Eric	L	Brown	M	5086 Nottingham Place	NULL	Duvall	79	Washington	98019	1961-01-08	2004-02-25	6
JChen	John	Y	Chen	M	3977 Central Avenue	NULL	Duvall	79	Washington	98019	1980-05-06	2003-03-13	0
JKane	John	T	Kane	M	8209 Green View Court	NULL	Duvall	79	Washington	98019	1980-10-29	2004-03-30	4
SHesse	Stefen	A	Hesse	M	8463 Vista Avenue	NULL	Duvall	79	Washington	98019	1970-01-21	2003-04-01	6
STejani	Sameer	A	Tejani	M	5379 Treasure Island Way	# 14	Duvall	79	Washington	98019	1972-07-27	2003-03-15	6
SRapier	Simon	D	Rapier	M	3421 Bouncing Road	NULL	Duvall	79	Washington	98019	1984-06-17	2003-01-09	3
EKogan	Eugene	O	Kogan	M	991 Vista Verde	NULL	Duvall	79	Washington	98019	1970-03-13	2003-03-12	7
DHite	Douglas	B	Hite	M	390 Ridgewood Ct.	NULL	Carnation	79	Washington	98014	1979-12-26	2003-01-28	7
DHall	Don	L	Hall	M	1411 Ranch Drive	NULL	Carnation	79	Washington	98014	1965-07-14	2003-03-17	4
JLiu	Jinghao	K	Liu	M	9666 Northridge Ct.	NULL	Carnation	79	Washington	98014	1983-03-09	2003-01-09	5
GLi	George	Z	Li	M	3074 Arbor Drive	NULL	Carnation	79	Washington	98014	1971-05-18	2003-01-22	8
KAbercrombie	Kim	B	Abercrombie	F	9752 Jeanne Circle	NULL	Carnation	79	Washington	98014	1961-01-14	2004-02-17	9
CPetculescu	Cristian	K	Petculescu	M	7166 Brock Lane	NULL	Seattle	79	Washington	98104	1978-05-13	2003-01-23	1
JDobney	JoLynn	M	Dobney	F	7126 Ending Ct.	NULL	Seattle	79	Washington	98104	1950-02-16	2002-01-26	7
EDudenhoefer	Ed	R	Dudenhoefer	M	4598 Manila Avenue	NULL	Seattle	79	Washington	98104	1965-10-12	2004-03-08	2
MNetz	Merav	A	Netz	F	5666 Hazelnut Lane	NULL	Seattle	79	Washington	98104	1977-06-13	2003-04-04	5
KKoenigsbauer	Kirk	J	Koenigsbauer	M	1220 Bradford Way	NULL	Seattle	79	Washington	98104	1979-03-10	2003-01-16	9
PMale	Pete	C	Male	M	5375 Clearland Circle	NULL	Seattle	79	Washington	98104	1971-03-07	2003-02-12	1
JCampbell	John	T	Campbell	M	2639 Anchor Court	NULL	Seattle	79	Washington	98104	1950-09-08	2002-04-18	0
YLi	Yuhong	L	Li	M	502 Alexander Pl.	NULL	Seattle	79	Washington	98104	1971-05-08	2003-03-05	2
BRettig	Bjorn	M	Rettig	M	5802 Ampersand Drive	NULL	Seattle	79	Washington	98104	1983-12-08	2003-02-08	1
JLugo	Jose	R	Lugo	M	5125 Cotton Ct.	NULL	Seattle	79	Washington	98104	1978-09-01	2003-03-14	1
NHolliday	Nicole	B	Holliday	F	3243 Buckingham Dr.	# 207	Seattle	79	Washington	98104	1980-05-10	2003-03-26	0
LSacksteder	Lane	M	Sacksteder	M	3029 Pastime Dr	# 2	Seattle	79	Washington	98104	1968-10-24	2003-02-12	6
PCook	Patrick	M	Cook	M	9537 Ridgewood Drive	NULL	Seattle	79	Washington	98104	1968-01-24	2004-03-15	4
TNusbaum	Tawana	G	Nusbaum	M	9964 North Ridge Drive	NULL	Seattle	79	Washington	98104	1983-12-12	2003-03-09	6
SReátegui Alayo	Sandra	NULL	Reátegui Alayo	F	1619 Stillman Court	NULL	Seattle	79	Washington	98104	1969-12-06	2003-01-27	4
PNartker	Paula	R	Nartker	F	2144 San Rafael	NULL	Seattle	79	Washington	98104	1981-03-13	2003-02-13	7
SGode	Scott	R	Gode	M	7403 N. Broadway	NULL	Seattle	79	Washington	98104	1981-03-13	2003-02-09	4
AStahl	Annik	O	Stahl	M	7842 Ygnacio Valley Road	NULL	Seattle	79	Washington	98104	1971-01-27	2003-01-18	6
JCreasey	Jack	T	Creasey	M	874 Olivera Road	NULL	Seattle	79	Washington	98104	1967-09-30	2004-04-03	1
RD'sa	Reuben	H	D'sa	M	1064 Slow Creek Road	NULL	Seattle	79	Washington	98104	1981-09-27	2003-01-16	6
LRandall	Linda	A	Randall	F	77 Birchwood	NULL	Seattle	79	Washington	98104	1971-11-06	2003-03-07	7
LPenuchot	Lionel	C	Penuchot	M	7765 Sunsine Drive	NULL	Seattle	79	Washington	98104	1982-04-15	2003-03-30	1
KKhanna	Karan	R	Khanna	M	1102 Ravenwood	NULL	Seattle	79	Washington	98104	1964-04-07	2004-01-23	5
SMasters	Steve	F	Masters	M	1398 Yorba Linda	NULL	Seattle	79	Washington	98104	1985-05-07	2003-03-19	3
KMcAskill-White	Katie	L	McAskill-White	F	4948 West 4th St	NULL	Seattle	79	Washington	98104	1978-12-20	2003-03-24	9
FLee	Frank	T	Lee	M	8290 Margaret Ct.	NULL	Seattle	79	Washington	98104	1981-10-07	2003-02-18	0
BCetinok	Baris	F	Cetinok	M	426 San Rafael	NULL	Seattle	79	Washington	98104	1984-11-07	2003-03-19	9
JEvans	John	P	Evans	M	136 Balboa Court	NULL	Seattle	79	Washington	98104	1972-07-01	2003-02-02	7
KKeil	Kendall	C	Keil	M	7439 Laguna Niguel	NULL	Seattle	79	Washington	98104	1980-06-30	2003-01-06	2
MRay	Michael	Sean	Ray	M	6498 Mining Rd.	NULL	Seattle	79	Washington	98104	1983-03-02	2003-03-19	9
ZMu	Zheng	W	Mu	M	6578 Woodhaven Ln.	NULL	Seattle	79	Washington	98104	1977-11-26	2003-01-04	2
MBaker	Mary	R	Baker	F	2354 Frame Ln.	NULL	Seattle	79	Washington	98104	1980-10-20	2004-01-26	9
RRounthwaite	Robert	J	Rounthwaite	M	6843 San Simeon Dr.	NULL	Seattle	79	Washington	98104	1979-04-01	2003-03-06	1
RHunter	Russell	NULL	Hunter	M	7616 Honey Court	NULL	Seattle	79	Washington	98104	1966-12-27	2003-01-13	3
DOrtiz	David	J	Ortiz	M	931 Corte De Luna	NULL	Seattle	79	Washington	98104	1979-01-30	2003-01-16	7
KLiu	Kevin	H	Liu	M	7594 Alexander Pl.	NULL	Seattle	79	Washington	98104	1980-01-26	2003-01-18	1
AMcGuel	Alejandro	E	McGuel	M	7127 Los Gatos Court	NULL	Seattle	79	Washington	98104	1983-01-06	2003-01-07	8
KZimmerman	Kimberly	B	Zimmerman	F	8656 Lakespring Place	NULL	Seattle	79	Washington	98104	1980-10-14	2004-02-13	4
DYalovsky	David	A	Yalovsky	M	5025 Holiday Hills	NULL	Seattle	79	Washington	98104	1975-09-04	2003-02-03	1
KLertpiriyasuwat	Kitti	H	Lertpiriyasuwat	F	5376 Catanzaro Way	NULL	Seattle	79	Washington	98104	1981-07-07	2003-04-05	3
TMaxwell	Taylor	R	Maxwell	M	504 O St.	NULL	Edmonds	79	Washington	98020	1950-05-03	2002-03-11	2
KSunkammurali	Krishna	NULL	Sunkammurali	M	6870 D Bel Air Drive	NULL	Edmonds	79	Washington	98020	1965-10-06	2004-03-16	1
LSong	Lolan	B	Song	M	8152 Claudia Dr.	NULL	Edmonds	79	Washington	98020	1967-02-25	2003-02-13	8
GYukish	Gary	W	Yukish	M	6057 Hill Street	NULL	Edmonds	79	Washington	98020	1982-06-17	2003-01-23	4
MVanderhyde	Michael	T	Vanderhyde	M	2812 Mazatlan	NULL	Edmonds	79	Washington	98020	1976-10-19	2003-03-30	8
JScardelis	Jim	H	Scardelis	M	172 Turning Dr.	NULL	Edmonds	79	Washington	98020	1980-10-09	2003-01-20	1
RSam	Raymond	K	Sam	M	9784 Mt Etna Drive	NULL	Edmonds	79	Washington	98020	1961-04-02	2003-01-24	1
LNay	Lorraine	O	Nay	F	2059 Clay Rd	NULL	Edmonds	79	Washington	98020	1982-12-28	2003-02-05	0
ABarbariol	Angela	W	Barbariol	F	2687 Ridge Road	NULL	Edmonds	79	Washington	98020	1985-07-01	2003-02-21	5
EGubbels	Eric	NULL	Gubbels	M	371 Apple Dr.	NULL	Edmonds	79	Washington	98020	1979-02-20	2003-02-15	9
SFatima	Suroor	R	Fatima	M	3281 Hillview Dr.	NULL	Edmonds	79	Washington	98020	1972-03-28	2003-01-18	3
MShoop	Margie	W	Shoop	F	2080 Sycamore Drive	NULL	Edmonds	79	Washington	98020	1980-06-20	2003-02-05	4
MZwilling	Michael	J	Zwilling	M	7511 Cooper Dr.	NULL	Edmonds	79	Washington	98020	1967-10-07	2004-03-26	9
MKrapauskas	Mindaugas	J	Krapauskas	M	9825 Coralie Drive	NULL	Edmonds	79	Washington	98020	1972-06-07	2003-02-14	8
CPoland	Carole	M	Poland	F	8411 Mt. Orange Place	NULL	Edmonds	79	Washington	98020	1977-11-19	2003-01-20	3
MRothkugel	Michael	L	Rothkugel	M	207 Berry Court	NULL	Edmonds	79	Washington	98020	1985-02-04	2003-02-11	1
GMares	Gabe	B	Mares	M	1061 Buskrik Avenue	NULL	Edmonds	79	Washington	98020	1982-06-11	2003-04-09	6
RReeves	Randy	T	Reeves	M	3632 Bank Way	NULL	Edmonds	79	Washington	98020	1964-05-29	2004-03-26	7
MDusza	Maciej	W	Dusza	M	3026 Anchor Drive	NULL	Edmonds	79	Washington	98020	1949-03-02	2004-03-01	0
TMichaels	Thomas	R	Michaels	M	7338 Green St.	NULL	Edmonds	79	Washington	98020	1980-02-11	2003-03-30	5
MPatten	Michael	W	Patten	M	2038 Encino Drive	NULL	Edmonds	79	Washington	98020	1968-06-03	2003-03-04	9
MFrintu	Mihail	U	Frintu	M	2466 Clearland Circle	NULL	Edmonds	79	Washington	98020	1965-04-09	2004-01-30	0
SHiga	Sidney	M	Higa	M	9277 Country View Lane	NULL	Edmonds	79	Washington	98020	1950-10-01	2002-03-05	4
RPatel	Rajesh	M	Patel	M	5423 Champion Rd.	NULL	Edmonds	79	Washington	98020	1971-11-05	2003-02-01	3
BSimon	Britta	L	Simon	F	2046 Las Palmas	NULL	Edmonds	79	Washington	98020	1983-10-30	2003-03-02	8
GAlderson	Greg	F	Alderson	M	8684 Military East	NULL	Bellevue	79	Washington	98004	1964-11-18	2003-01-03	8
PKomosinski	Paul	B	Komosinski	M	7270 Pepper Way	NULL	Bellevue	79	Washington	98004	1974-12-15	2003-01-05	3
KHomer	Kevin	M	Homer	M	6058 Hill Street	# 4	Bellevue	79	Washington	98004	1980-10-20	2004-01-26	5
SSmith	Samantha	H	Smith	F	1648 Eastgate Lane	NULL	Bellevue	79	Washington	98004	1981-12-23	2003-03-08	0
NYu	Nuan	NULL	Yu	M	3454 Bel Air Drive	NULL	Bellevue	79	Washington	98004	1973-04-29	2003-02-07	6
PWedge	Patrick	C	Wedge	M	3067 Maya	NULL	Bellevue	79	Washington	98004	1980-10-11	2004-03-04	7
RLaszlo	Rebecca	A	Laszlo	F	3197 Thornhill Place	NULL	Bellevue	79	Washington	98004	1971-08-11	2003-01-30	2
RKing	Russell	M	King	M	3919 Pinto Road	NULL	Bellevue	79	Washington	98004	1976-03-14	2003-03-25	8
MSu	Min	G	Su	M	7396 Stratton Circle	NULL	Bellevue	79	Washington	98004	1968-10-11	2004-02-25	2
SKim	Shane	S	Kim	M	9745 Bonita Ct.	NULL	Bellevue	79	Washington	98004	1984-06-24	2003-03-12	2
PKrebs	Peter	J	Krebs	M	3670 All Ways Drive	NULL	Bellevue	79	Washington	98004	1976-12-04	2003-01-02	5
OTurner	Olinda	C	Turner	F	7221 Peachwillow Street	NULL	Bellevue	79	Washington	98004	1964-05-05	2004-04-04	1
PSingh	Paul	R	Singh	M	1343 Prospect St	NULL	Bellevue	79	Washington	98004	1984-12-05	2003-02-18	0
SMunson	Stuart	V	Munson	M	6448 Castle Court	NULL	Bellevue	79	Washington	98004	1956-10-14	2003-01-03	3
SValdez	Sylvester	A	Valdez	M	7902 Grammercy Lane	Unit A	Bellevue	79	Washington	98004	1964-12-13	2004-01-12	7
TEminhizer	Terry	J	Eminhizer	M	8668 Via Neruda	NULL	Bellevue	79	Washington	98004	1980-03-07	2003-04-03	4
ARuth	Andy	M	Ruth	M	4777 Rockne Drive	NULL	Bellevue	79	Washington	98004	1977-11-20	2003-03-04	3
RHillmann	Reinout	N	Hillmann	M	620 Woodside Ct.	NULL	Bellevue	79	Washington	98004	1972-02-18	2005-01-25	3
MDempsey	Mary	A	Dempsey	F	6307 Greenbelt Way	NULL	Bellevue	79	Washington	98004	1972-03-01	2005-03-17	3
LMoschell	Linda	K	Moschell	F	3284 S. Blank Avenue	NULL	Bellevue	79	Washington	98004	1981-08-17	2003-01-26	7
FOgisu	Fukiko	J	Ogisu	M	8751 Norse Drive	NULL	Bellevue	79	Washington	98004	1964-12-25	2004-02-05	6
TEarls	Terrence	W	Earls	M	6968 Wren Ave.	NULL	Bellevue	79	Washington	98004	1979-01-09	2003-03-20	4
PBarreto de Mattos	Paula	M	Barreto de Mattos	F	4311 Clay Rd	NULL	Bellevue	79	Washington	98004	1970-03-14	2003-01-07	2
EHagens	Erin	M	Hagens	F	2947 Vine Lane	NULL	Bellevue	79	Washington	98004	1965-02-04	2004-03-03	4
JWatters	Jason	M	Watters	M	9320 Teakwood Dr.	NULL	Bellevue	79	Washington	98004	1983-01-08	2003-02-15	0
RShabalin	Rostislav	E	Shabalin	M	3711 Rollingwood Dr	NULL	Bellevue	79	Washington	98004	1971-10-15	2003-03-23	5
LMeisner	Linda	P	Meisner	F	6118 Grasswood Circle	NULL	Bellevue	79	Washington	98004	1964-12-31	2004-01-18	6
SMohan	Suchitra	O	Mohan	F	5678 Clear Court	NULL	Bellevue	79	Washington	98004	1981-07-11	2003-03-20	7
SMohamed	Shammi	G	Mohamed	M	332 Laguna Niguel	NULL	Bellevue	79	Washington	98004	1974-11-05	2003-01-25	3
GMatthew	Gigi	N	Matthew	F	7808 Brown St.	NULL	Bellevue	79	Washington	98004	1973-02-21	2003-02-17	7
WVong	William	S	Vong	M	6774 Bonanza	NULL	Bellevue	79	Washington	98004	1975-12-08	2003-02-08	5
KLoh	Kok-Ho	T	Loh	M	3708 Montana	NULL	Bellevue	79	Washington	98004	1974-05-30	2003-01-28	1
GErickson	Gail	A	Erickson	F	9435 Breck Court	NULL	Bellevue	79	Washington	98004	1946-10-29	2002-02-06	7
WBenshoof	Wanida	M	Benshoof	F	6951 Harmony Way	NULL	Sammamish	79	Washington	98074	1969-04-17	2005-02-07	5
JSheperdigian	Janet	L	Sheperdigian	F	6871 Thornwood Dr.	NULL	Sammamish	79	Washington	98074	1973-04-09	2003-04-02	0
SMetters	Susan	A	Metters	F	9104 Mt. Sequoia Ct.	NULL	Sammamish	79	Washington	98074	1977-05-03	2003-01-15	5
ACencini	Andrew	M	Cencini	M	4444 Pepper Way	NULL	Sammamish	79	Washington	98074	1982-10-26	2003-04-07	9
WKahn	Wendy	Beth	Kahn	F	4525 Benedict Ct.	NULL	Sammamish	79	Washington	98074	1978-11-12	2003-01-26	2
NAnderson	Nancy	A	Anderson	F	7820 Bird Drive	NULL	Sammamish	79	Washington	98074	1982-12-21	2003-02-03	8
MOsada	Michiko	F	Osada	M	1962 Ferndale Lane	NULL	Sammamish	79	Washington	98074	1976-07-28	2003-02-27	0
CFitzgerald	Charles	B	Fitzgerald	M	5263 Etcheverry Dr	NULL	Sammamish	79	Washington	98074	1965-10-03	2004-01-04	7
MIngle	Marc	J	Ingle	M	2473 Orchard Way	NULL	Sammamish	79	Washington	98074	1980-11-24	2003-02-17	4
NMirchandani	Nitin	S	Mirchandani	M	4096 San Remo	NULL	Sammamish	79	Washington	98074	1981-01-01	2003-01-29	3
HTing	Hung-Fu	T	Ting	M	7086 O St.	NULL	Sammamish	79	Washington	98074	1965-11-23	2004-02-07	5
SKaliyath	Sandeep	P	Kaliyath	M	4310 Kenston Dr.	NULL	Sammamish	79	Washington	98074	1965-01-03	2004-02-18	7
EZabokritski	Eugene	R	Zabokritski	M	7939 Bayview Court	NULL	Sammamish	79	Washington	98074	1981-08-15	2003-02-22	1
LKane	Lori	A	Kane	F	3066 Wallace Dr.	NULL	Redmond	79	Washington	98052	1974-08-19	2003-03-30	7
DGlimp	Diane	R	Glimp	F	9006 Woodside Way	NULL	Redmond	79	Washington	98052	1950-04-30	2002-04-29	8
JWood	John	L	Wood	M	9906 Oak Grove Road	NULL	Redmond	79	Washington	98052	1972-04-06	2005-03-10	0
JTrenary	Jean	E	Trenary	F	2383 Pepper Drive	NULL	Redmond	79	Washington	98052	1970-01-13	2003-01-12	4
AHill	Annette	L	Hill	F	6369 Ellis Street	NULL	Redmond	79	Washington	98052	1972-03-01	2005-01-06	7
BHeidepriem	Brandon	G	Heidepriem	M	8000 Crane Court	NULL	Redmond	79	Washington	98052	1971-02-11	2003-03-12	0
PSamarawickrama	Prasanna	E	Samarawickrama	M	9322 Driving Drive	NULL	Redmond	79	Washington	98052	1947-06-01	2004-02-23	7
DBradley	David	M	Bradley	M	3768 Door Way	NULL	Redmond	79	Washington	98052	1969-04-19	2002-01-20	9
BMiller	Ben	T	Miller	M	101 Candy Rd.	NULL	Redmond	79	Washington	98052	1967-07-05	2004-04-09	2
SJiang	Stephen	Y	Jiang	M	2427 Notre Dame Ave.	NULL	Redmond	79	Washington	98052	1945-11-17	2005-02-04	0
MMcArthur	Mark	K	McArthur	M	9863 Ridge Place	NULL	Redmond	79	Washington	98052	1973-10-26	2003-02-24	7
JGalvin	Janice	M	Galvin	F	3397 Rancho View Drive	NULL	Redmond	79	Washington	98052	1983-06-29	2005-01-23	9
JCao	Jun	T	Cao	M	4909 Poco Lane	NULL	Redmond	79	Washington	98052	1973-08-06	2003-01-15	6
HFeng	Hanying	P	Feng	M	7297 RisingView	NULL	Redmond	79	Washington	98052	1968-11-16	2003-01-17	4
RKoch	Reed	T	Koch	M	1275 West Street	NULL	Redmond	79	Washington	98052	1983-02-09	2003-03-06	3
RTamburello	Roberto	NULL	Tamburello	M	2137 Birchwood Dr	NULL	Redmond	79	Washington	98052	1968-12-13	2001-12-12	4
MEntin	Michael	T	Entin	M	2482 Buckingham Dr.	NULL	Redmond	79	Washington	98052	1983-07-17	2003-03-29	8
COkelberry	Chris	O	Okelberry	M	8467 Clifford Court	NULL	Redmond	79	Washington	98052	1980-09-07	2003-04-08	0
KFlood	Kathie	E	Flood	F	9241 St George Dr.	NULL	Everett	79	Washington	98201	1984-12-02	2003-02-28	1
MGibson	Mary	E	Gibson	F	3928 San Francisco	NULL	Everett	79	Washington	98201	1956-10-14	2003-02-13	3
DMargheim	Diane	L	Margheim	F	475 Santa Maria	NULL	Everett	79	Washington	98201	1980-07-06	2003-01-30	7
SSelikoff	Steven	T	Selikoff	M	181 Gaining Drive	NULL	Everett	79	Washington	98201	1971-06-15	2003-01-02	6
JHay	Jeff	V	Hay	M	3385 Crestview Drive	NULL	Everett	79	Washington	98201	1971-02-16	2003-02-22	3
SHarnpadoungsataya	Sariya	E	Harnpadoungsataya	M	1185 Dallas Drive	NULL	Everett	79	Washington	98201	1981-06-21	2003-01-13	5
KBrown	Kevin	F	Brown	M	7883 Missing Canyon Court	NULL	Everett	79	Washington	98201	1981-06-03	2001-02-26	0
WJohnson	Willis	T	Johnson	M	5452 Corte Gilberto	NULL	Everett	79	Washington	98201	1972-08-18	2003-01-14	7
TVande Velde	Tom	M	Vande Velde	M	5242 Marvelle Ln.	NULL	Everett	79	Washington	98201	1980-11-01	2004-04-10	5
JMiksovsky	Jan	S	Miksovsky	M	8624 Pepper Way	NULL	Everett	79	Washington	98201	1968-12-16	2003-04-06	4
REllerbrock	Ruth	Ann	Ellerbrock	F	2176 Apollo Way	NULL	Everett	79	Washington	98201	1950-07-06	2002-02-06	1
YMcKay	Yvonne	S	McKay	F	1962 Cotton Ct.	NULL	Everett	79	Washington	98201	1983-05-17	2003-01-10	1
KMyer	Ken	L	Myer	M	1362 Somerset Place	NULL	Everett	79	Washington	98201	1975-06-29	2003-03-28	2
JWilliams	Jill	A	Williams	F	3238 Laguna Circle	NULL	Everett	79	Washington	98201	1973-07-19	2003-02-19	3
JFrum	John	N	Frum	M	3665 Oak Creek Ct.	NULL	Everett	79	Washington	98201	1976-04-24	2003-04-04	2
DPoe	Deborah	E	Poe	F	7640 First Ave.	NULL	Everett	79	Washington	98201	1970-04-07	2003-01-19	6
AHill	Andrew	R	Hill	M	6629 Polson Circle	NULL	Everett	79	Washington	98201	1982-10-08	2003-03-26	5
BLaMee	Brian	P	LaMee	M	2294 West 39th St.	NULL	Everett	79	Washington	98201	1978-09-12	2003-04-04	5
ANayberg	Alex	M	Nayberg	M	1400 Gate Drive	NULL	Newport Hills	79	Washington	98006	1984-05-14	2003-03-12	4
KSánchez	Ken	J	Sánchez	M	4350 Minute Dr.	NULL	Newport Hills	79	Washington	98006	1963-03-02	2003-02-15	5
AWright	A. Scott	NULL	Wright	M	9297 Kenston Dr.	NULL	Newport Hills	79	Washington	98006	1962-10-19	2003-01-13	1
DBarber	David	M	Barber	M	8967 Hamilton Ave.	NULL	Newport Hills	79	Washington	98006	1958-07-23	2003-02-13	7
MMartin	Mindy	C	Martin	F	9687 Shakespeare Drive	NULL	Newport Hills	79	Washington	98006	1978-12-22	2003-01-26	0
BWalton	Bryan	A	Walton	M	1397 Paradise Ct.	NULL	Newport Hills	79	Washington	98006	1978-10-22	2003-02-25	1
VLuthra	Vidur	X	Luthra	M	3030 Blackburn Ct.	NULL	Newport Hills	79	Washington	98006	1978-09-02	2003-02-02	6
DLiu	David	J	Liu	M	9605 Pheasant Circle	NULL	Gold Bar	79	Washington	98251	1977-08-08	2003-03-03	3
PColeman	Pat	H	Coleman	M	2425 Notre Dame Ave	NULL	Gold Bar	79	Washington	98251	1965-01-03	2004-02-28	9
CKleinerman	Christian	E	Kleinerman	M	8036 Summit View Dr.	NULL	Gold Bar	79	Washington	98251	1970-02-18	2003-01-15	2
GCulbertson	Grant	N	Culbertson	M	213 Stonewood Drive	NULL	Gold Bar	79	Washington	98251	1970-05-18	2003-03-29	0
DTomic	Dragan	K	Tomic	M	3884 Beauty Street	# 14	Gold Bar	79	Washington	98251	1971-03-18	2003-03-15	7
JBerry	Jo	L	Berry	F	1748 Bird Drive	NULL	Index	79	Washington	98256	1948-05-25	2004-04-07	8
CSpoon	Candy	L	Spoon	F	310 Winter Lane	NULL	Index	79	Washington	98256	1970-03-26	2003-02-07	8
LPenor	Lori	K	Penor	F	3514 Sunshine	NULL	Index	79	Washington	98256	1964-08-31	2004-03-19	7
MSeamans	Mike	K	Seamans	M	1245 Clay Road	NULL	Index	79	Washington	98256	1973-08-01	2003-04-09	0
KBerge	Karen	R	Berge	F	3127 El Camino Drive	NULL	Index	79	Washington	98256	1970-01-25	2003-03-13	4
HChen	Hao	O	Chen	M	7691 Benedict Ct.	# 141	Issaquah	79	Washington	98027	1971-05-19	2003-03-10	3
FAjenstat	François	P	Ajenstat	M	1144 Paradise Ct.	NULL	Issaquah	79	Washington	98027	1969-06-17	2003-02-18	2
SConroy	Stephanie	A	Conroy	F	7435 Ricardo	NULL	Issaquah	79	Washington	98027	1978-04-26	2003-03-08	6
PConnelly	Peter	I	Connelly	M	9530 Vine Lane	NULL	Issaquah	79	Washington	98027	1974-06-29	2003-03-27	5
ASharma	Ashvini	R	Sharma	M	6580 Poor Ridge Court	NULL	Issaquah	79	Washington	98027	1971-04-28	2003-01-05	5
JBueno	Janaina	Barreiro Gambaro	Bueno	F	5979 El Pueblo	NULL	Issaquah	79	Washington	98027	1979-03-03	2003-01-24	2
DBacon	Dan	K	Bacon	M	1921 Ranch Road	NULL	Issaquah	79	Washington	98027	1975-07-28	2003-02-12	2
RMeyyappan	Ramesh	V	Meyyappan	M	3848 East 39th Street	NULL	Issaquah	79	Washington	98027	1982-04-14	2003-03-07	3
KBerg	Karen	A	Berg	F	5256 Chickpea Ct.	NULL	Issaquah	79	Washington	98027	1972-06-19	2003-03-20	0
CNorred	Chris	K	Norred	M	989 Crown Ct	NULL	Issaquah	79	Washington	98027	1981-06-26	2003-04-07	2
TKharatishvili	Tengiz	N	Kharatishvili	M	3333 Madhatter Circle	NULL	Issaquah	79	Washington	98027	1984-05-29	2003-01-17	5
ZArifin	Zainal	T	Arifin	M	342 San Simeon	NULL	Issaquah	79	Washington	98027	1970-03-02	2003-02-05	6
SChai	Sean	N	Chai	M	9314 Icicle Way	NULL	Issaquah	79	Washington	98027	1981-04-12	2003-02-23	5
BWelcker	Brian	S	Welcker	M	7772 Golden Meadow	NULL	Issaquah	79	Washington	98027	1971-07-08	2005-03-18	7
MHarrington	Mark	L	Harrington	M	8585 Los Gatos Ct.	NULL	Issaquah	79	Washington	98027	1980-05-31	2003-02-16	8
SAlexander	Sean	P	Alexander	M	7985 Center Street	NULL	Renton	79	Washington	98055	1970-04-07	2003-01-29	7
FPellow	Frank	S	Pellow	M	5980 Icicle Circle	Unit H	Renton	79	Washington	98055	1946-06-13	2004-02-24	4
EKurjan	Eric	S	Kurjan	M	1378 String Dr	NULL	Renton	79	Washington	98055	1966-10-19	2004-02-28	9
ARao	Arvind	B	Rao	M	9495 Limewood Place	NULL	Renton	79	Washington	98055	1968-09-21	2003-04-01	3
GAltman	Gary	E.	Altman	M	2598 Breck Court	NULL	Renton	79	Washington	98055	1965-03-21	2004-01-03	6
JGoldberg	Jossef	H	Goldberg	M	5670 Bel Air Dr.	NULL	Renton	79	Washington	98055	1953-04-11	2002-02-24	0
SSalavaria	Sharon	B	Salavaria	F	7165 Brock Lane	NULL	Renton	79	Washington	98055	1955-06-03	2005-02-18	2
SEaton	Susan	W	Eaton	F	2736 Scramble Rd	NULL	Renton	79	Washington	98055	1972-03-20	2003-01-08	8
SMacrae	Stuart	J	Macrae	M	2266 Greenwood Circle	NULL	Renton	79	Washington	98055	1966-01-17	2004-04-05	7
MHedlund	Magnus	E	Hedlund	M	9533 Working Drive	NULL	Renton	79	Washington	98055	1965-09-27	2004-01-22	1
ABerglund	Andreas	T	Berglund	M	1803 Olive Hill	NULL	Renton	79	Washington	98055	1983-04-29	2003-03-06	5
TDuffy	Terri	Lee	Duffy	F	7559 Worth Ct.	NULL	Renton	79	Washington	98055	1965-09-01	2002-03-03	0
MSullivan	Michael	I	Sullivan	M	6510 Hacienda Drive	NULL	Renton	79	Washington	98055	1973-07-17	2005-01-30	1
LNorman	Laura	F	Norman	F	6937 E. 42nd Street	NULL	Renton	79	Washington	98055	1970-02-06	2003-03-04	2
JBischoff	Jimmy	T	Bischoff	M	2176 Brown Street	NULL	Renton	79	Washington	98055	1979-06-05	2003-03-30	9
MBerndt	Matthias	T	Berndt	M	4312 Cambridge Drive	NULL	Renton	79	Washington	98055	1967-12-13	2003-02-21	4
AAlberts	Amy	E	Alberts	F	5009 Orange Street	NULL	Renton	79	Washington	98055	1951-10-22	2006-05-18	7
MRaheem	Michael	NULL	Raheem	M	1234 Seaside Way	NULL	San Francisco	9	California	94109	1979-01-01	2003-06-04	7
RWalters	Rob	NULL	Walters	M	5678 Lakeview Blvd.	NULL	Minneapolis	36	Minnesota	55402	1969-01-23	2002-01-05	3
OCracium	Ovidiu	V	Cracium	M	5458 Gladstone Drive	NULL	Kenmore	79	Washington	98028	1972-02-18	2005-01-05	8
PAckerman	Pilar	G	Ackerman	M	5407 Cougar Way	NULL	Seattle	79	Washington	98104	1966-10-11	2003-02-03	9
DCampbell	David	R	Campbell	M	2284 Azalea Avenue	NULL	Bellevue	79	Washington	98004	1968-03-14	2005-07-01	3
GHee	Gordon	L	Hee	M	108 Lakeside Court	NULL	Bellevue	79	Washington	98004	1960-12-30	2004-02-12	1
SWord	Sheela	H	Word	F	535 Greendell Pl	NULL	Sammamish	79	Washington	98074	1972-03-13	2005-03-28	4
BDiaz	Brenda	M	Diaz	F	1349 Steven Way	NULL	Seattle	79	Washington	98104	1977-03-31	2003-04-06	5
SCharncherngkha	Sootha	T	Charncherngkha	M	4155 Working Drive	NULL	Kenmore	79	Washington	98028	1961-01-05	2004-03-26	5
EErsan	Ebru	N	Ersan	M	8316 La Salle St.	NULL	Sammamish	79	Washington	98074	1980-10-23	2004-01-07	7
EKeyser	Elizabeth	I	Keyser	F	350 Pastel Drive	NULL	Kent	79	Washington	98031	1984-02-26	2003-04-03	6
HPournasseh	Houman	N	Pournasseh	M	9882 Clay Rde	NULL	Redmond	79	Washington	98052	1965-09-30	2003-02-26	7
HAbolrous	Hazem	E	Abolrous	M	5050 Mt. Wilson Way	NULL	Kenmore	79	Washington	98028	1971-11-27	2003-04-01	0
ASousa	Anibal	T	Sousa	F	6891 Ham Drive	NULL	Redmond	79	Washington	98052	1968-10-06	2003-03-27	1
RCornelsen	Ryan	L	Cornelsen	M	177 11th Ave	NULL	Sammamish	79	Washington	98074	1966-07-15	2003-02-06	8
SUddin	Sairaj	L	Uddin	M	8040 Hill Ct	NULL	Redmond	79	Washington	98052	1982-01-22	2003-02-27	4
BMoreland	Barbara	C	Moreland	F	137 Mazatlan	NULL	Seattle	79	Washington	98104	1970-02-04	2003-03-22	3
DWilson	Dan	B	Wilson	M	5863 Sierra	NULL	Bellevue	79	Washington	98004	1970-02-06	2003-02-23	5
BDecker	Barbara	S	Decker	F	7145 Matchstick Drive	NULL	Sammamish	79	Washington	98074	1973-08-02	2003-02-23	7";
        }
    }
}