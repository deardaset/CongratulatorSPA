import { useState, useEffect } from 'react';
import { getPeople } from "../api/personApi";

function getBirthdays(people, today = new Date(), sortBy = 'birthdate', search = '', daysAhead = 30) {
  const normaltoday = new Date(today.getFullYear(), today.getMonth(), today.getDate());
  const endDate = new Date(normaltoday);
  endDate.setDate(endDate.getDate() + daysAhead);

  const peopleWithNextBirthday = people.map(p => {
    const birthDate = new Date(p.birthDate);
    const thisYearBirthday = new Date(today.getFullYear(), birthDate.getMonth(), birthDate.getDate());

    if (thisYearBirthday < normaltoday) {
      thisYearBirthday.setFullYear(thisYearBirthday.getFullYear() + 1);
    }

    return { ...p, nextBirthday: thisYearBirthday };
  });

  const upcoming = peopleWithNextBirthday.filter(p => p.nextBirthday >= normaltoday && p.nextBirthday <= endDate);

  const normalizedSearch = search.trim().toLowerCase();
  return upcoming
    .filter(p => {
      if (!normalizedSearch) return true;

      return (
        p.name.toLowerCase().includes(normalizedSearch) ||
        p.relationshipType.toLowerCase().includes(normalizedSearch)
      );
    }).sort((a, b) => {
    switch (sortBy.toLowerCase()) {
      case 'name':
        return a.name.localeCompare(b.name);
      case 'age':
        return calculateAge(a.birthDate) - calculateAge(b.birthDate);
      case 'relationship':
        return a.relationshipType.localeCompare(b.relationshipType);
      case 'birthdate':
      default:
        return a.nextBirthday - b.nextBirthday;
    }
  });
}

function calculateAge(birthDate) {
    const today = new Date();
    const birthdate = new Date(birthDate);
    var age = today.getFullYear() - birthdate.getFullYear();

    const birthdayThisyear = new Date(
      today.getFullYear(),
      birthdate.getMonth(),
      birthdate.getDate()
    );

    if (today < birthdayThisyear) {
      --age;
    }
    return age;
}

const Home = () => {
  const [people, setPeople] = useState([]);
  const [sortBy, setSortBy] = useState('birthdate');
  const [search, setSearch] = useState('');

   useEffect(() => {
    getPeople()
      .then(data => {
        const sorted = getBirthdays(data, new Date(), sortBy, search, 30);
        setPeople(sorted);
      })
      .catch(err => console.error(err));
  }, [sortBy, search]);

  return (
    <main className='container'>
      <h1>Todays and upcoming birthdays</h1>      
      <div className='action-panel'>
        <label>
            Sort by:{' '}
            <select value={sortBy} onChange={e => setSortBy(e.target.value)}>
                <option value="name">Name</option>
                <option value="birthdate">Birthdate</option>
                <option value="age">Age</option>
                <option value="relationship">Relationship</option>
            </select>
        </label> 
        <label>
            Search:{' '}
            <input 
              type="text" 
              placeholder="Search" 
              value={search} 
              onChange={e => setSearch(e.target.value)} 
            /> 
        </label>
      </div>     
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Birthdate</th>
            <th>Age</th>
            <th>Relationship</th>
          </tr>
        </thead>
        <tbody>
          {people.map(p => (
            <tr key={p.guid}>
              <td>{p.name}</td>
              <td>{new Date(p.birthDate).toLocaleDateString('ru-RU')}</td>
              <td>{calculateAge(p.birthDate)}</td>
              <td>{p.relationshipType}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </main>
  )
}

export default Home