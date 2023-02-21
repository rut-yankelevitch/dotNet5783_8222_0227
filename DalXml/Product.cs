using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace Dal;
internal class Product : IProduct
{
    const string s_product = "product"; //Linq to XML

    static DO.Product? createProductfromXElement(XElement p)
    {
        return new DO.Product()
        {
            ID = p.ToIntNullable("ID") ?? throw new FormatException("id"), //fix to: DalXmlFormatException(id)),
            Category = p.ToEnumNullable<DO.Category>("Category"),
            Name = (string?)p.Element("Name"),
            Image = (string?)p.Element("Image"),
            Price = (double?)p.Element("Price"),
            InStock = p.ToIntNullable("InStock")
        };
    }
    public IEnumerable<DO.Student?> GetAll(Func<DO.Student?, bool>? filter = null)
    {
        XElement? studentsRootElem = XMLTools.LoadListFromXMLElement(s_students);

        //return studentsRootElem.Elements().Select(s => createStudentfromXElement(s)).Where(filter);

        if (filter != null)
        {
            return from s in studentsRootElem.Elements()
                   let doStud = createStudentfromXElement(s)
                   where filter(doStud)
                   select (DO.Student?)doStud;
        }
        else
        {
            return from s in studentsRootElem.Elements()
                   select createStudentfromXElement(s);
        }

    }

    public DO.Student GetById(int id)
    {
        XElement studentsRootElem = XMLTools.LoadListFromXMLElement(s_students);

        return (from s in studentsRootElem?.Elements()
                where s.ToIntNullable("ID") == id
                select (DO.Student?)createStudentfromXElement(s)).FirstOrDefault()
                ?? throw new Exception("missing id"); // fix to: throw new DalMissingIdException(id);
    }
    public int Add(DO.Student doStudent)
    {
        XElement studentsRootElem = XMLTools.LoadListFromXMLElement(s_students);

        XElement? stud = (from st in studentsRootElem.Elements()
                          where st.ToIntNullable("ID") == doStudent.ID //where (int?)st.Element("ID") == doStudent.ID
                          select st).FirstOrDefault();
        if (stud != null)
            throw new Exception("id already exist"); // fix to: throw new DalMissingIdException(id);

        XElement studentElem = new XElement("Student",
                                   new XElement("ID", doStudent.ID),
                                   new XElement("FirstName", doStudent.FirstName),
                                   new XElement("LastName", doStudent.LastName),
                                   new XElement("StudentStatus", doStudent.StudentStatus),
                                   new XElement("BirthDate", doStudent.BirthDate),
                                   new XElement("Grade", doStudent.Grade)
                                   );

        studentsRootElem.Add(studentElem);

        XMLTools.SaveListToXMLElement(studentsRootElem, s_students);

        return doStudent.ID; ;
    }

    public void Delete(int id)
    {
        XElement studentsRootElem = XMLTools.LoadListFromXMLElement(s_students);

        XElement? stud = (from st in studentsRootElem.Elements()
                          where (int?)st.Element("ID") == id
                          select st).FirstOrDefault() ?? throw new Exception("missing id"); // fix to: throw new DalMissingIdException(id);

        stud.Remove(); //<==>   Remove stud from studentsRootElem

        XMLTools.SaveListToXMLElement(studentsRootElem, s_students);
    }

    public void Update(DO.Student doStudent)
    {
        Delete(doStudent.ID);
        Add(doStudent);
    }

}
