using System;
using System.Collections.Generic;
//using System.Data.Entity.ModelConfiguration.Resources;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
	public class EnglishPluralizationService : PluralizationService, ICustomPluralizationMapping
	{
		private readonly BidirectionalDictionary<string, string> _userDictionary;
		private readonly StringBidirectionalDictionary _irregularPluralsPluralizationService;
		private readonly StringBidirectionalDictionary _assimilatedClassicalInflectionPluralizationService;
		private readonly StringBidirectionalDictionary _oSuffixPluralizationService;
		private readonly StringBidirectionalDictionary _classicalInflectionPluralizationService;
		private readonly StringBidirectionalDictionary _irregularVerbPluralizationService;
		private readonly StringBidirectionalDictionary _wordsEndingWithSePluralizationService;
		private readonly StringBidirectionalDictionary _wordsEndingWithSisPluralizationService;
		private readonly List<string> _knownSingluarWords;
		private readonly List<string> _knownPluralWords;
		private readonly string[] _uninflectiveSuffixes = new string[]
		{
			"fish", 
			"ois", 
			"sheep", 
			"deer", 
			"pos", 
			"itis", 
			"ism"
		};
		private readonly string[] _uninflectiveWords = new string[]
		{
			"bison", 
			"flounder", 
			"pliers", 
			"bream", 
			"gallows", 
			"proceedings", 
			"breeches", 
			"graffiti", 
			"rabies", 
			"britches", 
			"headquarters", 
			"salmon", 
			"carp", 
			"herpes", 
			"scissors", 
			"chassis", 
			"high-jinks", 
			"sea-bass", 
			"clippers", 
			"homework", 
			"series", 
			"cod", 
			"innings", 
			"shears", 
			"contretemps", 
			"jackanapes", 
			"species", 
			"corps", 
			"mackerel", 
			"swine", 
			"debris", 
			"measles", 
			"trout", 
			"diabetes", 
			"mews", 
			"tuna", 
			"djinn", 
			"mumps", 
			"whiting", 
			"eland", 
			"news", 
			"wildebeest", 
			"elk", 
			"pincers", 
			"police", 
			"hair", 
			"ice", 
			"chaos", 
			"milk", 
			"cotton", 
			"corn", 
			"millet", 
			"hay", 
			"pneumonoultramicroscopicsilicovolcanoconiosis", 
			"information", 
			"rice", 
			"tobacco", 
			"aircraft", 
			"rabies", 
			"scabies", 
			"diabetes", 
			"traffic", 
			"cotton", 
			"corn", 
			"millet", 
			"rice", 
			"hay", 
			"hemp", 
			"tobacco", 
			"cabbage", 
			"okra", 
			"broccoli", 
			"asparagus", 
			"lettuce", 
			"beef", 
			"pork", 
			"venison", 
			"bison", 
			"mutton", 
			"cattle", 
			"offspring", 
			"molasses", 
			"shambles", 
			"shingles"
		};
		private readonly Dictionary<string, string> _irregularVerbList = new Dictionary<string, string>
		{

			{
				"am", 
				"are"
			}, 

			{
				"are", 
				"are"
			}, 

			{
				"is", 
				"are"
			}, 

			{
				"was", 
				"were"
			}, 

			{
				"were", 
				"were"
			}, 

			{
				"has", 
				"have"
			}, 

			{
				"have", 
				"have"
			}
		};
		private readonly List<string> _pronounList = new List<string>
		{
			"I", 
			"we", 
			"you", 
			"he", 
			"she", 
			"they", 
			"it", 
			"me", 
			"us", 
			"him", 
			"her", 
			"them", 
			"myself", 
			"ourselves", 
			"yourself", 
			"himself", 
			"herself", 
			"itself", 
			"oneself", 
			"oneselves", 
			"my", 
			"our", 
			"your", 
			"his", 
			"their", 
			"its", 
			"mine", 
			"yours", 
			"hers", 
			"theirs", 
			"this", 
			"that", 
			"these", 
			"those", 
			"all", 
			"another", 
			"any", 
			"anybody", 
			"anyone", 
			"anything", 
			"both", 
			"each", 
			"other", 
			"either", 
			"everyone", 
			"everybody", 
			"everything", 
			"most", 
			"much", 
			"nothing", 
			"nobody", 
			"none", 
			"one", 
			"others", 
			"some", 
			"somebody", 
			"someone", 
			"something", 
			"what", 
			"whatever", 
			"which", 
			"whichever", 
			"who", 
			"whoever", 
			"whom", 
			"whomever", 
			"whose"
		};
		private readonly Dictionary<string, string> _irregularPluralsList = new Dictionary<string, string>
		{

			{
				"brother", 
				"brothers"
			}, 

			{
				"child", 
				"children"
			}, 

			{
				"cow", 
				"cows"
			}, 

			{
				"ephemeris", 
				"ephemerides"
			}, 

			{
				"genie", 
				"genies"
			}, 

			{
				"money", 
				"moneys"
			}, 

			{
				"mongoose", 
				"mongooses"
			}, 

			{
				"mythos", 
				"mythoi"
			}, 

			{
				"octopus", 
				"octopuses"
			}, 

			{
				"ox", 
				"oxen"
			}, 

			{
				"soliloquy", 
				"soliloquies"
			}, 

			{
				"trilby", 
				"trilbys"
			}, 

			{
				"crisis", 
				"crises"
			}, 

			{
				"synopsis", 
				"synopses"
			}, 

			{
				"rose", 
				"roses"
			}, 

			{
				"gas", 
				"gases"
			}, 

			{
				"bus", 
				"buses"
			}, 

			{
				"axis", 
				"axes"
			}, 

			{
				"memo", 
				"memos"
			}, 

			{
				"casino", 
				"casinos"
			}, 

			{
				"silo", 
				"silos"
			}, 

			{
				"stereo", 
				"stereos"
			}, 

			{
				"studio", 
				"studios"
			}, 

			{
				"lens", 
				"lenses"
			}, 

			{
				"alias", 
				"aliases"
			}, 

			{
				"pie", 
				"pies"
			}, 

			{
				"corpus", 
				"corpora"
			}, 

			{
				"viscus", 
				"viscera"
			}, 

			{
				"hippopotamus", 
				"hippopotami"
			}, 

			{
				"trace", 
				"traces"
			}, 

			{
				"person", 
				"people"
			}, 

			{
				"chilli", 
				"chillies"
			}, 

			{
				"analysis", 
				"analyses"
			}, 

			{
				"basis", 
				"bases"
			}, 

			{
				"neurosis", 
				"neuroses"
			}, 

			{
				"oasis", 
				"oases"
			}, 

			{
				"synthesis", 
				"syntheses"
			}, 

			{
				"thesis", 
				"theses"
			}, 

			{
				"pneumonoultramicroscopicsilicovolcanoconiosis", 
				"pneumonoultramicroscopicsilicovolcanoconioses"
			}, 

			{
				"status", 
				"statuses"
			}, 

			{
				"prospectus", 
				"prospectuses"
			}, 

			{
				"change", 
				"changes"
			}, 

			{
				"lie", 
				"lies"
			}, 

			{
				"calorie", 
				"calories"
			}, 

			{
				"freebie", 
				"freebies"
			}, 

			{
				"case", 
				"cases"
			}, 

			{
				"house", 
				"houses"
			}, 

			{
				"valve", 
				"valves"
			}, 

			{
				"cloth", 
				"clothes"
			}
		};
		private readonly Dictionary<string, string> _assimilatedClassicalInflectionList = new Dictionary<string, string>
		{

			{
				"alumna", 
				"alumnae"
			}, 

			{
				"alga", 
				"algae"
			}, 

			{
				"vertebra", 
				"vertebrae"
			}, 

			{
				"codex", 
				"codices"
			}, 

			{
				"murex", 
				"murices"
			}, 

			{
				"silex", 
				"silices"
			}, 

			{
				"aphelion", 
				"aphelia"
			}, 

			{
				"hyperbaton", 
				"hyperbata"
			}, 

			{
				"perihelion", 
				"perihelia"
			}, 

			{
				"asyndeton", 
				"asyndeta"
			}, 

			{
				"noumenon", 
				"noumena"
			}, 

			{
				"phenomenon", 
				"phenomena"
			}, 

			{
				"criterion", 
				"criteria"
			}, 

			{
				"organon", 
				"organa"
			}, 

			{
				"prolegomenon", 
				"prolegomena"
			}, 

			{
				"agendum", 
				"agenda"
			}, 

			{
				"datum", 
				"data"
			}, 

			{
				"extremum", 
				"extrema"
			}, 

			{
				"bacterium", 
				"bacteria"
			}, 

			{
				"desideratum", 
				"desiderata"
			}, 

			{
				"stratum", 
				"strata"
			}, 

			{
				"candelabrum", 
				"candelabra"
			}, 

			{
				"erratum", 
				"errata"
			}, 

			{
				"ovum", 
				"ova"
			}, 

			{
				"forum", 
				"fora"
			}, 

			{
				"addendum", 
				"addenda"
			}, 

			{
				"stadium", 
				"stadia"
			}, 

			{
				"automaton", 
				"automata"
			}, 

			{
				"polyhedron", 
				"polyhedra"
			}
		};
		private readonly Dictionary<string, string> _oSuffixList = new Dictionary<string, string>
		{

			{
				"albino", 
				"albinos"
			}, 

			{
				"generalissimo", 
				"generalissimos"
			}, 

			{
				"manifesto", 
				"manifestos"
			}, 

			{
				"archipelago", 
				"archipelagos"
			}, 

			{
				"ghetto", 
				"ghettos"
			}, 

			{
				"medico", 
				"medicos"
			}, 

			{
				"armadillo", 
				"armadillos"
			}, 

			{
				"guano", 
				"guanos"
			}, 

			{
				"octavo", 
				"octavos"
			}, 

			{
				"commando", 
				"commandos"
			}, 

			{
				"inferno", 
				"infernos"
			}, 

			{
				"photo", 
				"photos"
			}, 

			{
				"ditto", 
				"dittos"
			}, 

			{
				"jumbo", 
				"jumbos"
			}, 

			{
				"pro", 
				"pros"
			}, 

			{
				"dynamo", 
				"dynamos"
			}, 

			{
				"lingo", 
				"lingos"
			}, 

			{
				"quarto", 
				"quartos"
			}, 

			{
				"embryo", 
				"embryos"
			}, 

			{
				"lumbago", 
				"lumbagos"
			}, 

			{
				"rhino", 
				"rhinos"
			}, 

			{
				"fiasco", 
				"fiascos"
			}, 

			{
				"magneto", 
				"magnetos"
			}, 

			{
				"stylo", 
				"stylos"
			}
		};
		private readonly Dictionary<string, string> _classicalInflectionList = new Dictionary<string, string>
		{

			{
				"stamen", 
				"stamina"
			}, 

			{
				"foramen", 
				"foramina"
			}, 

			{
				"lumen", 
				"lumina"
			}, 

			{
				"anathema", 
				"anathemata"
			}, 

			{
				"enema", 
				"enemata"
			}, 

			{
				"oedema", 
				"oedemata"
			}, 

			{
				"bema", 
				"bemata"
			}, 

			{
				"enigma", 
				"enigmata"
			}, 

			{
				"sarcoma", 
				"sarcomata"
			}, 

			{
				"carcinoma", 
				"carcinomata"
			}, 

			{
				"gumma", 
				"gummata"
			}, 

			{
				"schema", 
				"schemata"
			}, 

			{
				"charisma", 
				"charismata"
			}, 

			{
				"lemma", 
				"lemmata"
			}, 

			{
				"soma", 
				"somata"
			}, 

			{
				"diploma", 
				"diplomata"
			}, 

			{
				"lymphoma", 
				"lymphomata"
			}, 

			{
				"stigma", 
				"stigmata"
			}, 

			{
				"dogma", 
				"dogmata"
			}, 

			{
				"magma", 
				"magmata"
			}, 

			{
				"stoma", 
				"stomata"
			}, 

			{
				"drama", 
				"dramata"
			}, 

			{
				"melisma", 
				"melismata"
			}, 

			{
				"trauma", 
				"traumata"
			}, 

			{
				"edema", 
				"edemata"
			}, 

			{
				"miasma", 
				"miasmata"
			}, 

			{
				"abscissa", 
				"abscissae"
			}, 

			{
				"formula", 
				"formulae"
			}, 

			{
				"medusa", 
				"medusae"
			}, 

			{
				"amoeba", 
				"amoebae"
			}, 

			{
				"hydra", 
				"hydrae"
			}, 

			{
				"nebula", 
				"nebulae"
			}, 

			{
				"antenna", 
				"antennae"
			}, 

			{
				"hyperbola", 
				"hyperbolae"
			}, 

			{
				"nova", 
				"novae"
			}, 

			{
				"aurora", 
				"aurorae"
			}, 

			{
				"lacuna", 
				"lacunae"
			}, 

			{
				"parabola", 
				"parabolae"
			}, 

			{
				"apex", 
				"apices"
			}, 

			{
				"latex", 
				"latices"
			}, 

			{
				"vertex", 
				"vertices"
			}, 

			{
				"cortex", 
				"cortices"
			}, 

			{
				"pontifex", 
				"pontifices"
			}, 

			{
				"vortex", 
				"vortices"
			}, 

			{
				"index", 
				"indices"
			}, 

			{
				"simplex", 
				"simplices"
			}, 

			{
				"iris", 
				"irides"
			}, 

			{
				"clitoris", 
				"clitorides"
			}, 

			{
				"alto", 
				"alti"
			}, 

			{
				"contralto", 
				"contralti"
			}, 

			{
				"soprano", 
				"soprani"
			}, 

			{
				"basso", 
				"bassi"
			}, 

			{
				"crescendo", 
				"crescendi"
			}, 

			{
				"tempo", 
				"tempi"
			}, 

			{
				"canto", 
				"canti"
			}, 

			{
				"solo", 
				"soli"
			}, 

			{
				"aquarium", 
				"aquaria"
			}, 

			{
				"interregnum", 
				"interregna"
			}, 

			{
				"quantum", 
				"quanta"
			}, 

			{
				"compendium", 
				"compendia"
			}, 

			{
				"lustrum", 
				"lustra"
			}, 

			{
				"rostrum", 
				"rostra"
			}, 

			{
				"consortium", 
				"consortia"
			}, 

			{
				"maximum", 
				"maxima"
			}, 

			{
				"spectrum", 
				"spectra"
			}, 

			{
				"cranium", 
				"crania"
			}, 

			{
				"medium", 
				"media"
			}, 

			{
				"speculum", 
				"specula"
			}, 

			{
				"curriculum", 
				"curricula"
			}, 

			{
				"memorandum", 
				"memoranda"
			}, 

			{
				"stadium", 
				"stadia"
			}, 

			{
				"dictum", 
				"dicta"
			}, 

			{
				"millenium", 
				"millenia"
			}, 

			{
				"trapezium", 
				"trapezia"
			}, 

			{
				"emporium", 
				"emporia"
			}, 

			{
				"minimum", 
				"minima"
			}, 

			{
				"ultimatum", 
				"ultimata"
			}, 

			{
				"enconium", 
				"enconia"
			}, 

			{
				"momentum", 
				"momenta"
			}, 

			{
				"vacuum", 
				"vacua"
			}, 

			{
				"gymnasium", 
				"gymnasia"
			}, 

			{
				"optimum", 
				"optima"
			}, 

			{
				"velum", 
				"vela"
			}, 

			{
				"honorarium", 
				"honoraria"
			}, 

			{
				"phylum", 
				"phyla"
			}, 

			{
				"focus", 
				"foci"
			}, 

			{
				"nimbus", 
				"nimbi"
			}, 

			{
				"succubus", 
				"succubi"
			}, 

			{
				"fungus", 
				"fungi"
			}, 

			{
				"nucleolus", 
				"nucleoli"
			}, 

			{
				"torus", 
				"tori"
			}, 

			{
				"genius", 
				"genii"
			}, 

			{
				"radius", 
				"radii"
			}, 

			{
				"umbilicus", 
				"umbilici"
			}, 

			{
				"incubus", 
				"incubi"
			}, 

			{
				"stylus", 
				"styli"
			}, 

			{
				"uterus", 
				"uteri"
			}, 

			{
				"stimulus", 
				"stimuli"
			}, 

			{
				"apparatus", 
				"apparatus"
			}, 

			{
				"impetus", 
				"impetus"
			}, 

			{
				"prospectus", 
				"prospectus"
			}, 

			{
				"cantus", 
				"cantus"
			}, 

			{
				"nexus", 
				"nexus"
			}, 

			{
				"sinus", 
				"sinus"
			}, 

			{
				"coitus", 
				"coitus"
			}, 

			{
				"plexus", 
				"plexus"
			}, 

			{
				"status", 
				"status"
			}, 

			{
				"hiatus", 
				"hiatus"
			}, 

			{
				"afreet", 
				"afreeti"
			}, 

			{
				"afrit", 
				"afriti"
			}, 

			{
				"efreet", 
				"efreeti"
			}, 

			{
				"cherub", 
				"cherubim"
			}, 

			{
				"goy", 
				"goyim"
			}, 

			{
				"seraph", 
				"seraphim"
			}, 

			{
				"alumnus", 
				"alumni"
			}
		};
		private readonly List<string> _knownConflictingPluralList = new List<string>
		{
			"they", 
			"them", 
			"their", 
			"have", 
			"were", 
			"yourself", 
			"are"
		};
		private readonly Dictionary<string, string> _wordsEndingWithSeList = new Dictionary<string, string>
		{

			{
				"house", 
				"houses"
			}, 

			{
				"case", 
				"cases"
			}, 

			{
				"enterprise", 
				"enterprises"
			}, 

			{
				"purchase", 
				"purchases"
			}, 

			{
				"surprise", 
				"surprises"
			}, 

			{
				"release", 
				"releases"
			}, 

			{
				"disease", 
				"diseases"
			}, 

			{
				"promise", 
				"promises"
			}, 

			{
				"refuse", 
				"refuses"
			}, 

			{
				"whose", 
				"whoses"
			}, 

			{
				"phase", 
				"phases"
			}, 

			{
				"noise", 
				"noises"
			}, 

			{
				"nurse", 
				"nurses"
			}, 

			{
				"rose", 
				"roses"
			}, 

			{
				"franchise", 
				"franchises"
			}, 

			{
				"supervise", 
				"supervises"
			}, 

			{
				"farmhouse", 
				"farmhouses"
			}, 

			{
				"suitcase", 
				"suitcases"
			}, 

			{
				"recourse", 
				"recourses"
			}, 

			{
				"impulse", 
				"impulses"
			}, 

			{
				"license", 
				"licenses"
			}, 

			{
				"diocese", 
				"dioceses"
			}, 

			{
				"excise", 
				"excises"
			}, 

			{
				"demise", 
				"demises"
			}, 

			{
				"blouse", 
				"blouses"
			}, 

			{
				"bruise", 
				"bruises"
			}, 

			{
				"misuse", 
				"misuses"
			}, 

			{
				"curse", 
				"curses"
			}, 

			{
				"prose", 
				"proses"
			}, 

			{
				"purse", 
				"purses"
			}, 

			{
				"goose", 
				"gooses"
			}, 

			{
				"tease", 
				"teases"
			}, 

			{
				"poise", 
				"poises"
			}, 

			{
				"vase", 
				"vases"
			}, 

			{
				"fuse", 
				"fuses"
			}, 

			{
				"muse", 
				"muses"
			}, 

			{
				"slaughterhouse", 
				"slaughterhouses"
			}, 

			{
				"clearinghouse", 
				"clearinghouses"
			}, 

			{
				"endonuclease", 
				"endonucleases"
			}, 

			{
				"steeplechase", 
				"steeplechases"
			}, 

			{
				"metamorphose", 
				"metamorphoses"
			}, 

			{
				"intercourse", 
				"intercourses"
			}, 

			{
				"commonsense", 
				"commonsenses"
			}, 

			{
				"intersperse", 
				"intersperses"
			}, 

			{
				"merchandise", 
				"merchandises"
			}, 

			{
				"phosphatase", 
				"phosphatases"
			}, 

			{
				"summerhouse", 
				"summerhouses"
			}, 

			{
				"watercourse", 
				"watercourses"
			}, 

			{
				"catchphrase", 
				"catchphrases"
			}, 

			{
				"compromise", 
				"compromises"
			}, 

			{
				"greenhouse", 
				"greenhouses"
			}, 

			{
				"lighthouse", 
				"lighthouses"
			}, 

			{
				"paraphrase", 
				"paraphrases"
			}, 

			{
				"mayonnaise", 
				"mayonnaises"
			}, 

			{
				"racecourse", 
				"racecourses"
			}, 

			{
				"apocalypse", 
				"apocalypses"
			}, 

			{
				"courthouse", 
				"courthouses"
			}, 

			{
				"powerhouse", 
				"powerhouses"
			}, 

			{
				"storehouse", 
				"storehouses"
			}, 

			{
				"glasshouse", 
				"glasshouses"
			}, 

			{
				"hypotenuse", 
				"hypotenuses"
			}, 

			{
				"peroxidase", 
				"peroxidases"
			}, 

			{
				"pillowcase", 
				"pillowcases"
			}, 

			{
				"roundhouse", 
				"roundhouses"
			}, 

			{
				"streetwise", 
				"streetwises"
			}, 

			{
				"expertise", 
				"expertises"
			}, 

			{
				"discourse", 
				"discourses"
			}, 

			{
				"warehouse", 
				"warehouses"
			}, 

			{
				"staircase", 
				"staircases"
			}, 

			{
				"workhouse", 
				"workhouses"
			}, 

			{
				"briefcase", 
				"briefcases"
			}, 

			{
				"clubhouse", 
				"clubhouses"
			}, 

			{
				"clockwise", 
				"clockwises"
			}, 

			{
				"concourse", 
				"concourses"
			}, 

			{
				"playhouse", 
				"playhouses"
			}, 

			{
				"turquoise", 
				"turquoises"
			}, 

			{
				"boathouse", 
				"boathouses"
			}, 

			{
				"cellulose", 
				"celluloses"
			}, 

			{
				"epitomise", 
				"epitomises"
			}, 

			{
				"gatehouse", 
				"gatehouses"
			}, 

			{
				"grandiose", 
				"grandioses"
			}, 

			{
				"menopause", 
				"menopauses"
			}, 

			{
				"penthouse", 
				"penthouses"
			}, 

			{
				"racehorse", 
				"racehorses"
			}, 

			{
				"transpose", 
				"transposes"
			}, 

			{
				"almshouse", 
				"almshouses"
			}, 

			{
				"customise", 
				"customises"
			}, 

			{
				"footloose", 
				"footlooses"
			}, 

			{
				"galvanise", 
				"galvanises"
			}, 

			{
				"princesse", 
				"princesses"
			}, 

			{
				"universe", 
				"universes"
			}, 

			{
				"workhorse", 
				"workhorses"
			}
		};
		private readonly Dictionary<string, string> _wordsEndingWithSisList = new Dictionary<string, string>
		{

			{
				"analysis", 
				"analyses"
			}, 

			{
				"crisis", 
				"crises"
			}, 

			{
				"basis", 
				"bases"
			}, 

			{
				"atherosclerosis", 
				"atheroscleroses"
			}, 

			{
				"electrophoresis", 
				"electrophoreses"
			}, 

			{
				"psychoanalysis", 
				"psychoanalyses"
			}, 

			{
				"photosynthesis", 
				"photosyntheses"
			}, 

			{
				"amniocentesis", 
				"amniocenteses"
			}, 

			{
				"metamorphosis", 
				"metamorphoses"
			}, 

			{
				"toxoplasmosis", 
				"toxoplasmoses"
			}, 

			{
				"endometriosis", 
				"endometrioses"
			}, 

			{
				"tuberculosis", 
				"tuberculoses"
			}, 

			{
				"pathogenesis", 
				"pathogeneses"
			}, 

			{
				"osteoporosis", 
				"osteoporoses"
			}, 

			{
				"parenthesis", 
				"parentheses"
			}, 

			{
				"anastomosis", 
				"anastomoses"
			}, 

			{
				"peristalsis", 
				"peristalses"
			}, 

			{
				"hypothesis", 
				"hypotheses"
			}, 

			{
				"antithesis", 
				"antitheses"
			}, 

			{
				"apotheosis", 
				"apotheoses"
			}, 

			{
				"thrombosis", 
				"thromboses"
			}, 

			{
				"diagnosis", 
				"diagnoses"
			}, 

			{
				"synthesis", 
				"syntheses"
			}, 

			{
				"paralysis", 
				"paralyses"
			}, 

			{
				"prognosis", 
				"prognoses"
			}, 

			{
				"cirrhosis", 
				"cirrhoses"
			}, 

			{
				"sclerosis", 
				"scleroses"
			}, 

			{
				"psychosis", 
				"psychoses"
			}, 

			{
				"apoptosis", 
				"apoptoses"
			}, 

			{
				"symbiosis", 
				"symbioses"
			}
		};
		public EnglishPluralizationService()
		{
			base.Culture = new CultureInfo("en");
			this._userDictionary = new BidirectionalDictionary<string, string>();
			this._irregularPluralsPluralizationService = new StringBidirectionalDictionary(this._irregularPluralsList);
			this._assimilatedClassicalInflectionPluralizationService = new StringBidirectionalDictionary(this._assimilatedClassicalInflectionList);
			this._oSuffixPluralizationService = new StringBidirectionalDictionary(this._oSuffixList);
			this._classicalInflectionPluralizationService = new StringBidirectionalDictionary(this._classicalInflectionList);
			this._wordsEndingWithSePluralizationService = new StringBidirectionalDictionary(this._wordsEndingWithSeList);
			this._wordsEndingWithSisPluralizationService = new StringBidirectionalDictionary(this._wordsEndingWithSisList);
			this._irregularVerbPluralizationService = new StringBidirectionalDictionary(this._irregularVerbList);
			this._knownSingluarWords = new List<string>(this._irregularPluralsList.Keys.Concat(this._assimilatedClassicalInflectionList.Keys).Concat(this._oSuffixList.Keys).Concat(this._classicalInflectionList.Keys).Concat(this._irregularVerbList.Keys).Concat(this._uninflectiveWords).Except(this._knownConflictingPluralList));
			this._knownPluralWords = new List<string>(this._irregularPluralsList.Values.Concat(this._assimilatedClassicalInflectionList.Values).Concat(this._oSuffixList.Values).Concat(this._classicalInflectionList.Values).Concat(this._irregularVerbList.Values).Concat(this._uninflectiveWords));
		}
		public override bool IsPlural(string word)
		{
			return this._userDictionary.ExistsInSecond(word) || (!this._userDictionary.ExistsInFirst(word) && (this.IsUninflective(word) || this._knownPluralWords.Contains(word.ToLower(base.Culture)) || !this.Singularize(word).Equals(word)));
		}
		public override bool IsSingular(string word)
		{
			return this._userDictionary.ExistsInFirst(word) || (!this._userDictionary.ExistsInSecond(word) && (this.IsUninflective(word) || this._knownSingluarWords.Contains(word.ToLower(base.Culture)) || (!this.IsNoOpWord(word) && this.Singularize(word).Equals(word))));
		}
		public override string Pluralize(string word)
		{
			return this.Capitalize(word, new Func<string, string>(this.publicPluralize));
		}
		private string publicPluralize(string word)
		{
			if (this._userDictionary.ExistsInFirst(word))
			{
				return this._userDictionary.GetSecondValue(word);
			}
			if (this.IsNoOpWord(word))
			{
				return word;
			}
			string str;
			string suffixWord = this.GetSuffixWord(word, out str);
			if (this.IsNoOpWord(suffixWord))
			{
				return str + suffixWord;
			}
			if (this.IsUninflective(suffixWord))
			{
				return str + suffixWord;
			}
			if (this._knownPluralWords.Contains(suffixWord.ToLowerInvariant()) || this.IsPlural(suffixWord))
			{
				return str + suffixWord;
			}
			if (this._irregularPluralsPluralizationService.ExistsInFirst(suffixWord))
			{
				return str + this._irregularPluralsPluralizationService.GetSecondValue(suffixWord);
			}
			string str2;
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"man"
			}, (string s) => s.Remove(s.Length - 2, 2) + "en", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"louse", 
				"mouse"
			}, (string s) => s.Remove(s.Length - 4, 4) + "ice", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"tooth"
			}, (string s) => s.Remove(s.Length - 4, 4) + "eeth", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"goose"
			}, (string s) => s.Remove(s.Length - 4, 4) + "eese", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"foot"
			}, (string s) => s.Remove(s.Length - 3, 3) + "eet", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"zoon"
			}, (string s) => s.Remove(s.Length - 3, 3) + "oa", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"cis", 
				"sis", 
				"xis"
			}, (string s) => s.Remove(s.Length - 2, 2) + "es", base.Culture, out str2))
			{
				return str + str2;
			}
			if (this._assimilatedClassicalInflectionPluralizationService.ExistsInFirst(suffixWord))
			{
				return str + this._assimilatedClassicalInflectionPluralizationService.GetSecondValue(suffixWord);
			}
			if (this._classicalInflectionPluralizationService.ExistsInFirst(suffixWord))
			{
				return str + this._classicalInflectionPluralizationService.GetSecondValue(suffixWord);
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"trix"
			}, (string s) => s.Remove(s.Length - 1, 1) + "ces", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"eau", 
				"ieu"
			}, (string s) => s + "x", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"inx", 
				"anx", 
				"ynx"
			}, (string s) => s.Remove(s.Length - 1, 1) + "ges", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"ch", 
				"sh", 
				"ss"
			}, (string s) => s + "es", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"alf", 
				"elf", 
				"olf", 
				"eaf", 
				"arf"
			}, delegate(string s)
			{
				if (!s.EndsWith("deaf", true, base.Culture))
				{
					return s.Remove(s.Length - 1, 1) + "ves";
				}
				return s;
			}
			, base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"nife", 
				"life", 
				"wife"
			}, (string s) => s.Remove(s.Length - 2, 2) + "ves", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"ay", 
				"ey", 
				"iy", 
				"oy", 
				"uy"
			}, (string s) => s + "s", base.Culture, out str2))
			{
				return str + str2;
			}
			if (suffixWord.EndsWith("y", true, base.Culture))
			{
				return str + suffixWord.Remove(suffixWord.Length - 1, 1) + "ies";
			}
			if (this._oSuffixPluralizationService.ExistsInFirst(suffixWord))
			{
				return str + this._oSuffixPluralizationService.GetSecondValue(suffixWord);
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"ao", 
				"eo", 
				"io", 
				"oo", 
				"uo"
			}, (string s) => s + "s", base.Culture, out str2))
			{
				return str + str2;
			}
			if (suffixWord.EndsWith("o", true, base.Culture))
			{
				return str + suffixWord + "es";
			}
			if (suffixWord.EndsWith("x", true, base.Culture))
			{
				return str + suffixWord + "es";
			}
			return str + suffixWord + "s";
		}
		public override string Singularize(string word)
		{
			return this.Capitalize(word, new Func<string, string>(this.publicSingularize));
		}
		private string publicSingularize(string word)
		{
			if (this._userDictionary.ExistsInSecond(word))
			{
				return this._userDictionary.GetFirstValue(word);
			}
			if (this.IsNoOpWord(word))
			{
				return word;
			}
			string str;
			string suffixWord = this.GetSuffixWord(word, out str);
			if (this.IsNoOpWord(suffixWord))
			{
				return str + suffixWord;
			}
			if (this.IsUninflective(suffixWord))
			{
				return str + suffixWord;
			}
			if (this._knownSingluarWords.Contains(suffixWord.ToLowerInvariant()))
			{
				return str + suffixWord;
			}
			if (this._irregularVerbPluralizationService.ExistsInSecond(suffixWord))
			{
				return str + this._irregularVerbPluralizationService.GetFirstValue(suffixWord);
			}
			if (this._irregularPluralsPluralizationService.ExistsInSecond(suffixWord))
			{
				return str + this._irregularPluralsPluralizationService.GetFirstValue(suffixWord);
			}
			if (this._wordsEndingWithSisPluralizationService.ExistsInSecond(suffixWord))
			{
				return str + this._wordsEndingWithSisPluralizationService.GetFirstValue(suffixWord);
			}
			if (this._wordsEndingWithSePluralizationService.ExistsInSecond(suffixWord))
			{
				return str + this._wordsEndingWithSePluralizationService.GetFirstValue(suffixWord);
			}
			string str2;
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"men"
			}, (string s) => s.Remove(s.Length - 2, 2) + "an", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"lice", 
				"mice"
			}, (string s) => s.Remove(s.Length - 3, 3) + "ouse", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"teeth"
			}, (string s) => s.Remove(s.Length - 4, 4) + "ooth", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"geese"
			}, (string s) => s.Remove(s.Length - 4, 4) + "oose", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"feet"
			}, (string s) => s.Remove(s.Length - 3, 3) + "oot", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"zoa"
			}, (string s) => s.Remove(s.Length - 2, 2) + "oon", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"ches", 
				"shes", 
				"sses"
			}, (string s) => s.Remove(s.Length - 2, 2), base.Culture, out str2))
			{
				return str + str2;
			}
			if (this._assimilatedClassicalInflectionPluralizationService.ExistsInSecond(suffixWord))
			{
				return str + this._assimilatedClassicalInflectionPluralizationService.GetFirstValue(suffixWord);
			}
			if (this._classicalInflectionPluralizationService.ExistsInSecond(suffixWord))
			{
				return str + this._classicalInflectionPluralizationService.GetFirstValue(suffixWord);
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"trices"
			}, (string s) => s.Remove(s.Length - 3, 3) + "x", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"eaux", 
				"ieux"
			}, (string s) => s.Remove(s.Length - 1, 1), base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"inges", 
				"anges", 
				"ynges"
			}, (string s) => s.Remove(s.Length - 3, 3) + "x", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"alves", 
				"elves", 
				"olves", 
				"eaves", 
				"arves"
			}, (string s) => s.Remove(s.Length - 3, 3) + "f", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"nives", 
				"lives", 
				"wives"
			}, (string s) => s.Remove(s.Length - 3, 3) + "fe", base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"ays", 
				"eys", 
				"iys", 
				"oys", 
				"uys"
			}, (string s) => s.Remove(s.Length - 1, 1), base.Culture, out str2))
			{
				return str + str2;
			}
			if (suffixWord.EndsWith("ies", true, base.Culture))
			{
				return str + suffixWord.Remove(suffixWord.Length - 3, 3) + "y";
			}
			if (this._oSuffixPluralizationService.ExistsInSecond(suffixWord))
			{
				return str + this._oSuffixPluralizationService.GetFirstValue(suffixWord);
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"aos", 
				"eos", 
				"ios", 
				"oos", 
				"uos"
			}, (string s) => suffixWord.Remove(suffixWord.Length - 1, 1), base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"ces"
			}, (string s) => s.Remove(s.Length - 1, 1), base.Culture, out str2))
			{
				return str + str2;
			}
			if (PluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
			{
				"ces", 
				"ses", 
				"xes"
			}, (string s) => s.Remove(s.Length - 2, 2), base.Culture, out str2))
			{
				return str + str2;
			}
			if (suffixWord.EndsWith("oes", true, base.Culture))
			{
				return str + suffixWord.Remove(suffixWord.Length - 2, 2);
			}
			if (suffixWord.EndsWith("ss", true, base.Culture))
			{
				return str + suffixWord;
			}
			if (suffixWord.EndsWith("s", true, base.Culture))
			{
				return str + suffixWord.Remove(suffixWord.Length - 1, 1);
			}
			return str + suffixWord;
		}
		private string Capitalize(string word, Func<string, string> action)
		{
			string text = action(word);
			if (!this.IsCapitalized(word))
			{
				return text;
			}
			if (text.Length == 0)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			stringBuilder.Append(char.ToUpperInvariant(text[0]));
			stringBuilder.Append(text.Substring(1));
			return stringBuilder.ToString();
		}
		private string GetSuffixWord(string word, out string prefixWord)
		{
			int num = word.LastIndexOf(' ');
			prefixWord = word.Substring(0, num + 1);
			return word.Substring(num + 1);
		}
		private bool IsCapitalized(string word)
		{
			return !string.IsNullOrEmpty(word) && char.IsUpper(word, 0);
		}
		private bool IsAlphabets(string word)
		{
			return !string.IsNullOrEmpty(word.Trim()) && word.Equals(word.Trim()) && !Regex.IsMatch(word, "[^a-zA-Z\\s]");
		}
		private bool IsUninflective(string word)
		{
			return PluralizationServiceUtil.DoesWordContainSuffix(word, this._uninflectiveSuffixes, base.Culture) || (!word.ToLower(base.Culture).Equals(word) && word.EndsWith("ese", false, base.Culture)) || this._uninflectiveWords.Contains(word.ToLowerInvariant());
		}
		private bool IsNoOpWord(string word)
		{
			return !this.IsAlphabets(word) || word.Length <= 1 || this._pronounList.Contains(word.ToLowerInvariant());
		}
		public void AddWord(string singular, string plural)
		{
			if (this._userDictionary.ExistsInSecond(plural))
			{
				//throw new ArgumentException(Strings.DuplicateEntryInUserDictionary("plural", plural), "plural");
			    throw new ArgumentException(String.Format("Duplicate Entry In User Dictionary Plural :", plural));
			}
			if (this._userDictionary.ExistsInFirst(singular))
			{
                //throw new ArgumentException(Strings.DuplicateEntryInUserDictionary("singular", singular), "singular");
                throw new ArgumentException(String.Format("Duplicate Entry In User Dictionary Singular :", singular));
			}
			this._userDictionary.AddValue(singular, plural);
		}
	}
}
