import json

f = open("levels.json")
text = f.read()
f.close()

data = json.loads(text)


for level in data["levels"]:
    res = []
    for key in level["levelObjects"]:
        a = { "name": key, "chance": level["levelObjects"][key]["chance"]}
        res.append(a)
    level["levelObjects"] = res

    res = []
    for key in level["lutObjects"]:
        a = { "name": key, "chance": level["lutObjects"][key]["chance"]}
        res.append(a)
        
    level["lutObjects"] = res
    
level = data["endless"]
res = []
for key in level["levelObjects"]:
    a = { "name": key, "chance": level["levelObjects"][key]["chance"], "minSpeed": level["levelObjects"][key]["minSpeed"]}
    res.append(a)
level["levelObjects"] = res

res = []
for key in level["lutObjects"]:
    a = { "name": key, "chance": level["lutObjects"][key]["chance"]}
    res.append(a)
level["lutObjects"] = res

print(json.dumps(data))


