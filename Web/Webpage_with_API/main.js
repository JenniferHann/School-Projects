const urlR1 = "https://api.spacexdata.com/v3/rockets/falcon1";
const urlR2 = "https://api.spacexdata.com/v3/rockets/falcon9";
const urlR3 = "https://api.spacexdata.com/v3/rockets/falconheavy";
const urlR4 = "https://api.spacexdata.com/v3/rockets/starship";
const urlPL = "https://api.spacexdata.com/v3/launches/past?limit=10";
const urlUL = "https://api.spacexdata.com/v3/launches/next";

let data;


fetch(urlR1)
    .then(response => response.json())
    .then(data => {
        getData(data)
    })

    .catch(function(error) {
        console.log(error);
    });

function getData(data) {
	displayButtons(data);
	displayContent1(data);
}

function displayButtons(data)
{
	//******************************************************************************
	//code from Vikram Singh with my modifications
	let tabContainer = document.getElementsByTagName('main')[0];
	let tabs = tabContainer.getElementsByTagName('section');
	let buttons = [];

	for (const tab of tabs) {
		let tabName = tab.getAttribute('data-tab-name');
		let tabButtonElementNode = document.createElement('button');
		let tabButtonTextNode = document.createTextNode(tabName);

		tabButtonElementNode.setAttribute('tab-name', tabName);
		tabButtonElementNode.appendChild(tabButtonTextNode);
		tabButtonElementNode.addEventListener('click', tabClicked);
	
		tabContainer.prepend(tabButtonElementNode);

		tab.style.display = 'none';
		buttons.push(tabButtonElementNode);
	}

	for(let i=0; i < tabs.length; i++)
	{
    	buttons[i].style.border = 'none';
    	buttons[i].style.backgroundColor = 'rgba(0, 0,0, 0.6)';
		buttons[i].style.color = 'white';
		buttons[i].style.padding = '15px 32px';
		buttons[i].style.textAlign = 'center';
		buttons[i].style.textDecoration = 'none';
		buttons[i].style.display = 'inline-block';
		buttons[i].style.fontSize = '16px';
	}

	tabs[2].style.display = 'block';
	buttons[2].style.backgroundColor = 'rgba(255,255,255, 0.6)';
	buttons[2].style.color = 'black';
	buttons[2].style.padding = '15px 32px';
	//******************************************************************************

	//******************************************************************************
	//code from Vikram Singh with my modifications
	function tabClicked(event) {
		let button = event.target;
		let tabName = button.getAttribute('tab-name');

		for (let i = 0; i < tabs.length; i++) {
			if (tabs[i].getAttribute('data-tab-name') == tabName) {
				tabs[i].style.display = 'block';
				buttons[i].style.backgroundColor = 'rgba(255,255,255, 0.6)';
				buttons[i].style.color = 'black';
				buttons[i].style.padding = '15px 32px';
				tabs[i].style.overflowY = 'scroll';

				if(tabName == "Past Launch")
				{
					PastLaunchClick();
				}

				if(tabName == "Upcomings Launch")
				{
					UpcomingsLaunchClick();
				}
			}
			else {
				tabs[i].style.display = 'none';
				buttons[i].style.backgroundColor = 'rgba(0,0,0, 0.6)';
				buttons[i].style.color = 'white';
				buttons[i].style.padding = '15px 32px';
				tabs[i].style.overflowY = 'none';
			}
		}
	}
	//******************************************************************************
}

let pastLaunchClick = false;
function PastLaunchClick()
{
	fetch(urlPL)
		.then(response => response.json())
		.then(data4 => {
			getData(data4)
		})

		.catch(function(error) {
			console.log(error);
		});

	function getData(data4) {
		if(pastLaunchClick == false)
		{
			pastLaunchClick = true;
							
			for(let i=0; i<data4.length; i++)
			{
				let launch = document.getElementById('PLtable');
				let row = document.createElement('tr');

				let missionNameText = document.createTextNode(data4[i].mission_name);
				let launchDateText = document.createTextNode(data4[i].launch_date_local);
				let rocketNameText = document.createTextNode(data4[i].rocket.rocket_name);
				let detailsText = document.createTextNode(data4[i].details);
				let launchSiteText = document.createTextNode(data4[i].launch_site.site_name_long);
				let launchSuccessText = document.createTextNode(data4[i].launch_success);

				let missionName = document.createElement('td');
				let launchDate = document.createElement('td');
				let rocketName = document.createElement('td');
				let details = document.createElement('td');
				let launchSite = document.createElement('td');
				let launchSuccess = document.createElement('td');

				missionName.append(missionNameText);
				launchDate.append(launchDateText);
				rocketName.append(rocketNameText);
				details.append(detailsText);
				launchSite.append(launchSiteText);
				launchSuccess.append(launchSuccessText);

				row.append(missionName);
				row.append(launchDate);
				row.append(rocketName);
				row.append(details);
				row.append(launchSite);
				row.append(launchSuccess);

				launch.append(row);
			}
		}
	}
}

function UpcomingsLaunchClick()
{
	fetch(urlUL)
    .then(response => response.json())
    .then(data5 => {
        getData(data5)
    })

    .catch(function(error) {
        console.log(error);
    });

	function getData(data5) {
		console.log(data5);
		let launch = document.getElementById('ULtable');
		let row = document.createElement('tr');

		let missionNameText = document.createTextNode(data5.mission_name);
		let launchDateText = document.createTextNode(data5.launch_date_local);
		let rocketNameText = document.createTextNode(data5.rocket.rocket_name);
		let launchSiteText = document.createTextNode(data5.launch_site.site_name_long);

		let missionName = document.createElement('td');
		let launchDate = document.createElement('td');
		let rocketName = document.createElement('td');
		let launchSite = document.createElement('td');

		missionName.append(missionNameText);
		launchDate.append(launchDateText);
		rocketName.append(rocketNameText);
		launchSite.append(launchSiteText);

		row.append(missionName);
		row.append(launchDate);
		row.append(rocketName);
		row.append(launchSite);

		launch.append(row);
	}
}

//first rocket
let rNameText;
let rIdText;
let rMassText;
let rHeightText;
let rDescriptionText;

let rName;
let rId;
let rDescription;

let name ;

let idLabel = document.createTextNode("ID: ");
let massLabel = document.createTextNode("Mass: ");
let heightLabel = document.createTextNode("Height: ");
let descriptionLabel = document.createTextNode("Description: ");

let kg = document.createTextNode("kg");
let m = document.createTextNode("m");
//********************* 

function displayContent1(data) {
	rNameText = document.createTextNode(data.rocket_name);
	rIdText = document.createTextNode(data.rocket_id);
	rMassText = document.createTextNode(data.mass.kg);
	rHeightText = document.createTextNode(data.height.meters);
	rDescriptionText = document.createTextNode(data.description);

	rId = document.createElement('p');
	rMass = document.createElement('p');
	rHeight = document.createElement('p');
	rDescription = document.createElement('p');

	name = document.getElementById('rInfo1');
	name.append(rNameText);
}

function displayContent3(data2)
{
	name = document.getElementById('rInfo3');
	rNameText2 = document.createTextNode(data2.rocket_name);
	rName2 = document.createElement('p');
	rName2.append(rNameText1);
	name.append(rName2);

	rId2 = document.createElement('p');
	rMass2 = document.createElement('p');
	rHeight2 = document.createElement('p');

	rIdText2 = document.createTextNode(data2.rocket_id);
	rMassText2 = document.createTextNode(data2.mass.kg);
	rHeightText2 = document.createTextNode(data2.height.meters);

	rName2 = document.createElement('p');
	rName2.append(rName2);
}

function rocket1()
{
	let info = document.getElementById('b1');
	let nameBox1 = document.getElementById('text1');
	let nameBox2 = document.getElementById('text2');
	nameBox1.style.top = "30%";
	nameBox2.style.top = "30%";

	rId.append(idLabel);
	rId.append(rIdText);

	rMass.append(massLabel);
	rMass.append(rMassText);
	rMass.append(kg);

	rHeight.append(heightLabel);
	rHeight.append(rHeightText);
	rHeight.append(m);

	rDescription.append(descriptionLabel);
	rDescription.append(rDescriptionText);
	rDescription.style.width = "450px";
	rDescription.style.margin = "0px";

	info.append(rId);
	info.append(rMass);
	info.append(rHeight);
	info.append(rDescription);
}

//second rocket
let isDataDisplay2 = false;

function rocket2()
{
	if(isDataDisplay2 == false)
	{
		isDataDisplay2 = true;

		let idLabel1 = document.createTextNode("ID: ");
		let massLabel1 = document.createTextNode("Mass: ");
		let heightLabel1 = document.createTextNode("Height: ");
		let descriptionLabel1 = document.createTextNode("Description: ");

		let kg1 = document.createTextNode("kg");
		let m1 = document.createTextNode("m");

		fetch(urlR2)
			.then(response => response.json())
			.then(data1 => {
				getData1(data1)
			})

			.catch(function(error) {
				console.log(error);
			});

		function getData1(data1) {
			let rIdText1 = document.createTextNode(data1.rocket_id);
			let rMassText1 = document.createTextNode(data1.mass.kg);
			let rHeightText1 = document.createTextNode(data1.height.meters);
			let rDescriptionText1 = document.createTextNode(data1.description);

			let rId1 = document.createElement('p');
			let rMass1 = document.createElement('p');
			let rHeight1 = document.createElement('p');
			let rDescription1 = document.createElement('p');
		
			let info1 = document.getElementById('b2');
			let nameBox1 = document.getElementById('text1');
			let nameBox2 = document.getElementById('text2');
			nameBox1.style.top = "30%";
			nameBox2.style.top = "30%";

			rId1.append(idLabel1);
			rId1.append(rIdText1);

			rMass1.append(massLabel1);
			rMass1.append(rMassText1);
			rMass1.append(kg1);

			rHeight1.append(heightLabel1);
			rHeight1.append(rHeightText1);
			rHeight1.append(m1);
			
			rDescription1.append(descriptionLabel1);
			rDescription1.append(rDescriptionText1);
			rDescription1.style.width = "450px";

			info1.append(rId1);
			info1.append(rMass1);
			info1.append(rHeight1);
			info1.append(rDescription1);
		}
	}
}

//third rocket
let dataIsDisplay1 = false;

function rocket3()
{
	if(dataIsDisplay1 == false)
	{
		dataIsDisplay1 = true;

		let idLabel2 = document.createTextNode("ID: ");
		let massLabel2 = document.createTextNode("Mass: ");
		let heightLabel2 = document.createTextNode("Height: ");
		let descriptionLabel2 = document.createTextNode("Description: ");

		let kg2 = document.createTextNode("kg");
		let m2 = document.createTextNode("m");

		fetch(urlR3)
		.then(response => response.json())
		.then(data2 => {
			getData1(data2)
		})

		.catch(function(error) {
			console.log(error);
		});

		function getData1(data2) {
			
			let rId2 = document.createElement('p');
			let rMass2 = document.createElement('p');
			let rHeight2 = document.createElement('p');
			let rDescription2 = document.createElement('p');

			let rIdText2 = document.createTextNode(data2.rocket_id);
			let rMassText2 = document.createTextNode(data2.mass.kg);
			let rHeightText2 = document.createTextNode(data2.height.meters);
			let rDescriptionText2 = document.createTextNode(data2.description);

			let info2 = document.getElementById('b3');
			let nameBox3 = document.getElementById('text3');
			let nameBox4 = document.getElementById('text4');
			nameBox3.style.top = "30%";
			nameBox4.style.top = "30%";
			
			rId2.append(idLabel2);
			rId2.append(rIdText2);

			rMass2.append(massLabel2);
			rMass2.append(rMassText2);
			rMass2.append(kg2);

			rHeight2.append(heightLabel2);
			rHeight2.append(rHeightText2);
			rHeight2.append(m2);

			rDescription2.append(descriptionLabel2);
			rDescription2.append(rDescriptionText2);
			rDescription2.style.width = "450px";

			info2.append(rId2);
			info2.append(rMass2);
			info2.append(rHeight2);		
			info2.append(rDescription2);	
		}
	}		
}

//fourth rocket
let dataIsDisplay2 = false;

function rocket4()
{
	if(dataIsDisplay2 == false)
	{
		dataIsDisplay2 = true;

		let idLabel3 = document.createTextNode("ID: ");
		let massLabel3 = document.createTextNode("Mass: ");
		let heightLabel3 = document.createTextNode("Height: ");
		let descriptionLabel3 = document.createTextNode("Description: ");

		let kg3 = document.createTextNode("kg");
		let m3 = document.createTextNode("m");

		fetch(urlR4)
		.then(response => response.json())
		.then(data3 => {
			getData1(data3)
		})

		.catch(function(error) {
			console.log(error);
		});

		function getData1(data3) {
			
			let rId3 = document.createElement('p');
			let rMass3 = document.createElement('p');
			let rHeight3 = document.createElement('p');
			let rDescription3 = document.createElement('p');

			let rIdText3 = document.createTextNode(data3.rocket_id);
			let rMassText3 = document.createTextNode(data3.mass.kg);
			let rHeightText3 = document.createTextNode(data3.height.meters);
			let rDescriptionText3 = document.createTextNode(data3.description);

			let info3 = document.getElementById('b4');
			let nameBox3 = document.getElementById('text3');
			let nameBox4 = document.getElementById('text4');
			nameBox3.style.top = "30%";
			nameBox4.style.top = "30%";
			
			rId3.append(idLabel3);
			rId3.append(rIdText3);

			rMass3.append(massLabel3);
			rMass3.append(rMassText3);
			rMass3.append(kg3);

			rHeight3.append(heightLabel3);
			rHeight3.append(rHeightText3);
			rHeight3.append(m3);

			rDescription3.append(descriptionLabel3);
			rDescription3.append(rDescriptionText3);
			rDescription3.style.width = "450px";

			info3.append(rId3);
			info3.append(rMass3);
			info3.append(rHeight3);			
			info3.append(rDescription3);
		}
	}
}

function displayPL()
{
	let input = document.getElementById('PL-filter').value;
	let launch = document.getElementById('PLtable');
	let url = "https://api.spacexdata.com/v3/launches/past?limit=";
	let urlPLFilter = "";
	
	urlPLFilter = url.concat(input);

	while(launch.firstChild)
	{
		launch.removeChild(launch.firstChild);
	}
	
	if(input >0)
	{
		fetch(urlPLFilter)
			.then(response => response.json())
			.then(data5 => {
				getData(data5)
			})

			.catch(function(error) {
				console.log(error);
			});

		function getData(data5) {
			
			for(let i=0; i<data5.length; i++)
			{
				let row = document.createElement('tr');

				let missionNameText = document.createTextNode(data5[i].mission_name);
				let launchDateText = document.createTextNode(data5[i].launch_date_local);
				let rocketNameText = document.createTextNode(data5[i].rocket.rocket_name);
				let detailsText = document.createTextNode(data5[i].details);
				let launchSiteText = document.createTextNode(data5[i].launch_site.site_name_long);
				let launchSuccessText = document.createTextNode(data5[i].launch_success);

				let missionName = document.createElement('td');
				let launchDate = document.createElement('td');
				let rocketName = document.createElement('td');
				let details = document.createElement('td');
				let launchSite = document.createElement('td');
				let launchSuccess = document.createElement('td');

				missionName.append(missionNameText);
				launchDate.append(launchDateText);
				rocketName.append(rocketNameText);
				details.append(detailsText);
				launchSite.append(launchSiteText);
				launchSuccess.append(launchSuccessText);

				row.append(missionName);
				row.append(launchDate);
				row.append(rocketName);
				row.append(details);
				row.append(launchSite);
				row.append(launchSuccess);

				launch.append(row);
			}
		}
	}
}