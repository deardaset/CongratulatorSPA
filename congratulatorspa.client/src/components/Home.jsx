import { useState, useEffect } from 'react';
import { getPeople } from "../api/personApi";

function getBirthdays(people, today = new Date(), sortBy = 'birthdate', daysAhead = 30) {
  const endDate = new Date(today);
  endDate.setDate(endDate.getDate() + daysAhead);

  const peopleWithNextBirthday = people.map(p => {
    const birthDate = new Date(p.birthDate);
    const thisYearBirthday = new Date(today.getFullYear(), birthDate.getMonth(), birthDate.getDate());

    if (thisYearBirthday < today) {
      thisYearBirthday.setFullYear(thisYearBirthday.getFullYear() + 1);
    }

    return { ...p, nextBirthday: thisYearBirthday };
  });

  const upcoming = peopleWithNextBirthday.filter(p => p.nextBirthday >= today && p.nextBirthday <= endDate);

  return upcoming.sort((a, b) => {
    switch (sortBy.toLowerCase()) {
      case 'name':
        return a.name.localeCompare(b.name);
      case 'relationship':
        return a.relationshipType.localeCompare(b.relationshipType);
      case 'birthdate':
      default:
        return a.nextBirthday - b.nextBirthday;
    }
  });
}

const Home = () => {
  const [people, setPeople] = useState([]);
  const [sortBy, setSortBy] = useState('birthdate');

   useEffect(() => {
    getPeople()
      .then(data => {
        const sorted = getBirthdays(data, new Date(), sortBy, 30);
        setPeople(sorted);
      })
      .catch(err => console.error(err));
  }, [sortBy]);

  return (
    <main className='container'>
      <h1>Todays and upcoming birthdays</h1>      
      <div className='sort'>
        <label>
            Sort by:{' '}
            <select value={sortBy} onChange={e => setSortBy(e.target.value)}>
                <option value="name">Name</option>
                <option value="birthdate">Birthdate</option>
                <option value="relationship">Relationship</option>
            </select>
        </label> 
      </div>     
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Birthdate</th>
            <th>Relationship</th>
          </tr>
        </thead>
        <tbody>
          {people.map(p => (
            <tr key={p.guid}>
              <td>{p.name}</td>
              <td>{new Date(p.birthDate).toLocaleDateString('ru-RU')}</td>
              <td>{p.relationshipType}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </main>
  )
}

export default Home