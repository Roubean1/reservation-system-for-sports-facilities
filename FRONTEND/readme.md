### SportCentrum Ostrava – Dokumentace k Frontend aplikaci (Blazor WebAssembly)



##### PŘEHLED FUNKCIONALIT:



1. Layout a navigace



* Implementován globální Navbar (navigace), Main Container pro obsah a Footer s informacemi.



* Responzivní design pro mobilní zařízení i desktopy.



2\. Domovská stránka



* Úvodní rozcestník pro uživatele s informacemi o sportovním centru.



3\. Seznam sportů



* Kompletní přehled nabízených aktivit (název, popis, seznam navázaných sportovišť).



* Dynamické prokliky na detaily konkrétních sportovišť, kde lze daný sport provozovat.



* Profesionální fotodokumentace pro každý sport (Tenis, Badminton, Squash, Basketbal, Volejbal, Futsal, Stolní tenis).



4\. Seznam sportovišť



* Přehled všech hal, kurtů a ploch.



* Zobrazení dostupných sportů pro každé sportoviště a prokliky na jejich detail.



5\. Detail sportoviště



* Kompletní informace: název areálu, přesná adresa, podporované sporty a ceník.



* Výpis aktuálních rezervací pro dané místo.



* Přímý proklik na vytvoření rezervace s automatickým předvyplněním daného sportoviště.



6\. Půjčovna vybavení



* Samostatný katalog s profesionálními fotografiemi (rakety, míče, dresy).



* Informace o ceně za hodinu a aktuální dostupnosti na skladě.



7\. Rezervace



* Inteligentní formulář s dynamickým výběrem sportu (nabízí pouze sporty dostupné na daném sportovišti).



* Podpora query parametrů pro automatické předvyplnění údajů z detailu sportoviště.



8\. Login a zabezpečení



* Autentizace pomocí JWT (JSON Web Token).



* Bezpečné ukládání tokenu a dat uživatele v localStorage (využití knihovny Blazored.LocalStorage).



* Logout funkce zajišťující kompletní smazání citlivých dat z prohlížeče.



9\. Registrace



* Funkční vytváření nových uživatelských účtů přes API.



10\. Profil uživatele



* Možnost editace osobních a kontaktních údajů uživatele.



* Správa rezervací: zobrazení historie i budoucích termínů s možností jejich zrušení (storno).



* Přehled zapůjčeného vybavení k aktuálním rezervacím.



* Zobrazení support ticketů pro technickou podporu.



* Informace o stavu členství (Membership).







##### POUŽITÉ TECHNOLOGIE:



* Frontend: Blazor WebAssembly (.NET 8.0).



* Správa stavu: Blazored.LocalStorage.



* Komunikace: HttpClient (JSON).



* Ikony a grafika: FontAwesome 6, custom realistické AI generované fotografie.







##### TECHNICKÉ POZNÁMKY:



* BaseAddress: URL pro API je definována v souboru Program.cs.



* Protokoly: API i Frontend musí běžet na stejném protokolu (např. oba na http), aby nedocházelo k chybám v komunikaci.








##### STRUKTURA PROJEKTU:



* Pages/: Razor komponenty jednotlivých stránek.



* DTO/: Datové modely pro komunikaci s API.



* wwwroot/imgs/sports/: Fotografie sportovních aktivit.



* wwwroot/imgs/equipment/: Fotografie vybavení.

